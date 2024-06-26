// dllmain.cpp : 定义 DLL 应用程序的入口点。
#include "pch.h"
#include "socksify_unmanaged.h"

BOOL APIENTRY DllMain(HMODULE hModule,
    DWORD  ul_reason_for_call,
    LPVOID lpReserved
)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

// Convert event_type_mx to JSON
void to_json(nlohmann::json& j, const event_type_mx& p) {
    j = static_cast<uint32_t>(p);
}

// Convert JSON to event_type_mx
void from_json(const nlohmann::json& j, event_type_mx& p) {
    p = static_cast<event_type_mx>(j.get<uint32_t>());
}

// Convert event_mx to JSON
void to_json(nlohmann::json& j, const event_mx& p) {
    j = nlohmann::json{ {"time",p.time}, { "type", p.type }, {"data", p.data}, {"msg",p.msg}};
}


EXTERN_C __declspec(dllexport) void __stdcall socksify_init(int level)
{
    socksify_unmanaged::get_instance((log_level_mx)level);
}

EXTERN_C __declspec(dllexport) LONG_PTR __stdcall add_socks5_proxy(const char* endpoint,
    const int protocol,
    const bool start,
    const char* login,
    const char* password)
{
	return socksify_unmanaged::get_instance()->add_socks5_proxy(endpoint, (supported_protocols_mx)protocol, start, login, password);
}

EXTERN_C __declspec(dllexport) bool __stdcall start()
{
	return socksify_unmanaged::get_instance()->start();
}

EXTERN_C __declspec(dllexport) bool __stdcall stop()
{
	return socksify_unmanaged::get_instance()->stop();
}

EXTERN_C __declspec(dllexport) bool __stdcall associate_process_name_to_proxy(const wchar_t* process_name, LONG_PTR proxy_id)
{
	return socksify_unmanaged::get_instance()->associate_process_name_to_proxy(process_name, proxy_id);
}

EXTERN_C __declspec(dllexport) void __stdcall set_log_limit(uint32_t log_limit)
{
	socksify_unmanaged::get_instance()->set_log_limit(log_limit);
}

EXTERN_C __declspec(dllexport) uint32_t __stdcall get_log_limit()
{
	return socksify_unmanaged::get_instance()->get_log_limit();
}

EXTERN_C __declspec(dllexport) void __stdcall set_log_event(HANDLE log_event)
{
	socksify_unmanaged::get_instance()->set_log_event(log_event);
}

EXTERN_C __declspec(dllexport) const char* __stdcall read_log()
{
	auto logs=socksify_unmanaged::get_instance()->read_log();
    nlohmann::json j = logs;
    static std::string str = j.dump();
    return str.c_str();
}