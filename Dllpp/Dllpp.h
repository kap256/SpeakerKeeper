#pragma once

#define WIN32_LEAN_AND_MEAN             // Windows ヘッダーから使用されていない部分を除外します。
// Windows ヘッダー ファイル:
#include <windows.h>

const int MY_OK = 0;
const int MY_ERROR = -1;

const int MY_CAPTURED = 10;
const int MY_NO_DATA = 20;


void OnError(HRESULT ehr);
int WINAPI InitAudioStream();
int WINAPI GetSleepMSec();
int WINAPI CaptureAudioStream();
void WINAPI DisposeAudioStream();