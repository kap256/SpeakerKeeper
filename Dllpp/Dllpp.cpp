//
// https://docs.microsoft.com/en-us/windows/win32/coreaudio/capturing-a-stream
//

#include "stdafx.h"

// REFERENCE_TIME time units per second and per millisecond
#define REFTIMES_PER_SEC  10000000
#define REFTIMES_PER_MILLISEC  10000

#define EXIT_ON_ERROR(hres)  \
              {if (FAILED(hres)) { throw hres; }}
#define SAFE_RELEASE(punk)  \
              if ((punk) != NULL)  \
                { (punk)->Release(); (punk) = NULL; }

const CLSID CLSID_MMDeviceEnumerator = __uuidof(MMDeviceEnumerator);
const IID IID_IMMDeviceEnumerator = __uuidof(IMMDeviceEnumerator);
const IID IID_IAudioClient = __uuidof(IAudioClient);
const IID IID_IAudioCaptureClient = __uuidof(IAudioCaptureClient);


WAVEFORMATEX* pwfx = NULL;
IMMDeviceEnumerator* pEnumerator = NULL;
IMMDevice* pDevice = NULL;
IAudioClient* pAudioClient = NULL;
REFERENCE_TIME hnsActualDuration;
IAudioCaptureClient* pCaptureClient = NULL;


int WINAPI InitAudioStream()
{
	HRESULT hr;
	REFERENCE_TIME hnsRequestedDuration = REFTIMES_PER_SEC;
	UINT32 bufferFrameCount;

	try {
		CoInitialize(NULL);

		hr = CoCreateInstance(
			CLSID_MMDeviceEnumerator, NULL,
			CLSCTX_ALL, IID_IMMDeviceEnumerator,
			(void**)&pEnumerator);
		EXIT_ON_ERROR(hr);;

		hr = pEnumerator->GetDefaultAudioEndpoint(
			eRender, eConsole, &pDevice);
		EXIT_ON_ERROR(hr);

		hr = pDevice->Activate(
			IID_IAudioClient, CLSCTX_ALL,
			NULL, (void**)&pAudioClient);
		EXIT_ON_ERROR(hr);

		hr = pAudioClient->GetMixFormat(&pwfx);
		EXIT_ON_ERROR(hr);

		hr = pAudioClient->Initialize(
			AUDCLNT_SHAREMODE_SHARED,
			AUDCLNT_STREAMFLAGS_LOOPBACK,
			hnsRequestedDuration,
			0,
			pwfx,
			NULL);
		EXIT_ON_ERROR(hr);

		// Get the size of the allocated buffer.
		hr = pAudioClient->GetBufferSize(&bufferFrameCount);
		EXIT_ON_ERROR(hr);

		hr = pAudioClient->GetService(
			IID_IAudioCaptureClient,
			(void**)&pCaptureClient);
		EXIT_ON_ERROR(hr);

		// Notify the audio sink which format to use.
		//hr = pMySink->SetFormat(pwfx);
		EXIT_ON_ERROR(hr);

		// Calculate the actual duration of the allocated buffer.
		hnsActualDuration = (double)REFTIMES_PER_SEC *
			bufferFrameCount / pwfx->nSamplesPerSec;

		hr = pAudioClient->Start();  // Start recording.
		EXIT_ON_ERROR(hr);
	} catch (HRESULT ehr) {
		OnError(ehr);
		DisposeAudioStream();
		return MY_ERROR;
	}
	return MY_OK;
}
int WINAPI GetSleepMSec()
{
	return int(hnsActualDuration / REFTIMES_PER_MILLISEC / 2);
}
int WINAPI CaptureAudioStream()
{
	HRESULT hr;
	UINT32 packetLength = 0;
	BYTE* pData;
	UINT32 numFramesAvailable;
	DWORD flags;

	try {
		// Each loop fills about half of the shared buffer.
		// Sleep for half the buffer duration.
		
		hr = pCaptureClient->GetNextPacketSize(&packetLength);
		EXIT_ON_ERROR(hr);

		if (packetLength == 0) {
			return MY_NO_DATA;
		}
		int ret = MY_NO_DATA;
		while (packetLength != 0)
		{
			// Get the available data in the shared buffer.
			hr = pCaptureClient->GetBuffer(
				&pData,
				&numFramesAvailable,
				&flags, NULL, NULL);
			EXIT_ON_ERROR(hr);

			if (!(flags & AUDCLNT_BUFFERFLAGS_SILENT))	{
				ret = MY_CAPTURED;
			}

			// Copy the available capture data to the audio sink.
			//hr = pMySink->CopyData(
			//                  pData, numFramesAvailable, &bDone);
			//EXIT_ON_ERROR(hr);

			hr = pCaptureClient->ReleaseBuffer(numFramesAvailable);
			EXIT_ON_ERROR(hr);

			hr = pCaptureClient->GetNextPacketSize(&packetLength);
			EXIT_ON_ERROR(hr);
		}
		return ret;
	} catch (HRESULT ehr) {
		OnError(ehr);
		DisposeAudioStream();
		return MY_ERROR;
	}
}

void WINAPI DisposeAudioStream()
{
	HRESULT hr;
	try {
		hr = pAudioClient->Stop();  // Stop recording.
		EXIT_ON_ERROR(hr);
	} catch (HRESULT ehr) {
		OnError(ehr);
	}

	CoTaskMemFree(pwfx);
	SAFE_RELEASE(pEnumerator);
	SAFE_RELEASE(pDevice);
	SAFE_RELEASE(pAudioClient);
	SAFE_RELEASE(pCaptureClient);
	CoUninitialize();
}


void OnError(HRESULT ehr)
{

	WCHAR hr_message[512];
	auto ret = FormatMessage(
		FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_MAX_WIDTH_MASK,
		NULL,
		ehr,
		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
		hr_message,
		std::size(hr_message) - 1,
		NULL);

	if (ret == 0) {
		switch (ehr) {
		case AUDCLNT_E_NOT_INITIALIZED:
			wcscpy(hr_message, L"AUDCLNT_E_NOT_INITIALIZED          "); break;
		case AUDCLNT_E_ALREADY_INITIALIZED:
			wcscpy(hr_message, L"AUDCLNT_E_ALREADY_INITIALIZED          "); break;
		case AUDCLNT_E_WRONG_ENDPOINT_TYPE:
			wcscpy(hr_message, L"AUDCLNT_E_WRONG_ENDPOINT_TYPE          "); break;
		case AUDCLNT_E_DEVICE_INVALIDATED:
			wcscpy(hr_message, L"AUDCLNT_E_DEVICE_INVALIDATED           "); break;
		case AUDCLNT_E_NOT_STOPPED:
			wcscpy(hr_message, L"AUDCLNT_E_NOT_STOPPED                  "); break;
		case AUDCLNT_E_BUFFER_TOO_LARGE:
			wcscpy(hr_message, L"AUDCLNT_E_BUFFER_TOO_LARGE             "); break;
		case AUDCLNT_E_OUT_OF_ORDER:
			wcscpy(hr_message, L"AUDCLNT_E_OUT_OF_ORDER                 "); break;
		case AUDCLNT_E_UNSUPPORTED_FORMAT:
			wcscpy(hr_message, L"AUDCLNT_E_UNSUPPORTED_FORMAT           "); break;
		case AUDCLNT_E_INVALID_SIZE:
			wcscpy(hr_message, L"AUDCLNT_E_INVALID_SIZE                 "); break;
		case AUDCLNT_E_DEVICE_IN_USE:
			wcscpy(hr_message, L"AUDCLNT_E_DEVICE_IN_USE                "); break;
		case AUDCLNT_E_BUFFER_OPERATION_PENDING:
			wcscpy(hr_message, L"AUDCLNT_E_BUFFER_OPERATION_PENDING     "); break;
		case AUDCLNT_E_THREAD_NOT_REGISTERED:
			wcscpy(hr_message, L"AUDCLNT_E_THREAD_NOT_REGISTERED        "); break;
		case AUDCLNT_E_EXCLUSIVE_MODE_NOT_ALLOWED:
			wcscpy(hr_message, L"AUDCLNT_E_EXCLUSIVE_MODE_NOT_ALLOWED   "); break;
		case AUDCLNT_E_ENDPOINT_CREATE_FAILED:
			wcscpy(hr_message, L"AUDCLNT_E_ENDPOINT_CREATE_FAILED       "); break;
		case AUDCLNT_E_SERVICE_NOT_RUNNING:
			wcscpy(hr_message, L"AUDCLNT_E_SERVICE_NOT_RUNNING          "); break;
		case AUDCLNT_E_EVENTHANDLE_NOT_EXPECTED:
			wcscpy(hr_message, L"AUDCLNT_E_EVENTHANDLE_NOT_EXPECTED     "); break;
		case AUDCLNT_E_EXCLUSIVE_MODE_ONLY:
			wcscpy(hr_message, L"AUDCLNT_E_EXCLUSIVE_MODE_ONLY          "); break;
		case AUDCLNT_E_BUFDURATION_PERIOD_NOT_EQUAL:
			wcscpy(hr_message, L"AUDCLNT_E_BUFDURATION_PERIOD_NOT_EQUAL "); break;
		case AUDCLNT_E_EVENTHANDLE_NOT_SET:
			wcscpy(hr_message, L"AUDCLNT_E_EVENTHANDLE_NOT_SET          "); break;
		case AUDCLNT_E_INCORRECT_BUFFER_SIZE:
			wcscpy(hr_message, L"AUDCLNT_E_INCORRECT_BUFFER_SIZE        "); break;
		case AUDCLNT_E_BUFFER_SIZE_ERROR:
			wcscpy(hr_message, L"AUDCLNT_E_BUFFER_SIZE_ERROR            "); break;
		case AUDCLNT_E_CPUUSAGE_EXCEEDED:
			wcscpy(hr_message, L"AUDCLNT_E_CPUUSAGE_EXCEEDED            "); break;
		case AUDCLNT_E_BUFFER_ERROR:
			wcscpy(hr_message, L"AUDCLNT_E_BUFFER_ERROR                 "); break;
		case AUDCLNT_E_BUFFER_SIZE_NOT_ALIGNED:
			wcscpy(hr_message, L"AUDCLNT_E_BUFFER_SIZE_NOT_ALIGNED      "); break;
		case AUDCLNT_E_INVALID_DEVICE_PERIOD:
			wcscpy(hr_message, L"AUDCLNT_E_INVALID_DEVICE_PERIOD        "); break;
		case AUDCLNT_E_INVALID_STREAM_FLAG:
			wcscpy(hr_message, L"AUDCLNT_E_INVALID_STREAM_FLAG          "); break;
		case AUDCLNT_E_ENDPOINT_OFFLOAD_NOT_CAPABLE:
			wcscpy(hr_message, L"AUDCLNT_E_ENDPOINT_OFFLOAD_NOT_CAPABLE "); break;
		case AUDCLNT_E_OUT_OF_OFFLOAD_RESOURCES:
			wcscpy(hr_message, L"AUDCLNT_E_OUT_OF_OFFLOAD_RESOURCES     "); break;
		case AUDCLNT_E_OFFLOAD_MODE_ONLY:
			wcscpy(hr_message, L"AUDCLNT_E_OFFLOAD_MODE_ONLY            "); break;
		case AUDCLNT_E_NONOFFLOAD_MODE_ONLY:
			wcscpy(hr_message, L"AUDCLNT_E_NONOFFLOAD_MODE_ONLY         "); break;
		case AUDCLNT_E_RESOURCES_INVALIDATED:
			wcscpy(hr_message, L"AUDCLNT_E_RESOURCES_INVALIDATED        "); break;
		case AUDCLNT_E_RAW_MODE_UNSUPPORTED:
			wcscpy(hr_message, L"AUDCLNT_E_RAW_MODE_UNSUPPORTED         "); break;
		case AUDCLNT_E_ENGINE_PERIODICITY_LOCKED:
			wcscpy(hr_message, L"AUDCLNT_E_ENGINE_PERIODICITY_LOCKED    "); break;
		case AUDCLNT_E_ENGINE_FORMAT_LOCKED:
			wcscpy(hr_message, L"AUDCLNT_E_ENGINE_FORMAT_LOCKED         "); break;
		case AUDCLNT_E_HEADTRACKING_ENABLED:
			wcscpy(hr_message, L"AUDCLNT_E_HEADTRACKING_ENABLED         "); break;
		case AUDCLNT_E_HEADTRACKING_UNSUPPORTED:
			wcscpy(hr_message, L"AUDCLNT_E_HEADTRACKING_UNSUPPORTED     "); break;
		}
	}
	wprintf(L"%d:%s\n", ehr, hr_message);
	OutputDebugStringW(hr_message);
}