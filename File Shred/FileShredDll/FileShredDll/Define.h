#pragma once
#include <iostream>
#include <string>
#include <time.h>
#include <io.h>
#include <windows.h>
#include <vector>
#include <winioctl.h>


#define WM_UPDATE_PROGRESS			WM_USER + 1000
#define WM_UPDATE_SHRED_RESULT		WM_USER + 1001
#define WM_UPDATE_WIPE_PROCESS		WM_USER + 1002

#define SUCCESS 1
#define FAILE 0
#define CLEAN_BUF_SIZE 0x100000  
#define MAX_PER_BUFSIZE 0x2000000  //32 MB
#define HIGH_BYTE(x)  (((x) & 0xf0)>>4)
#define LOW_BYTE(x)   ((x) & 0x0f)
