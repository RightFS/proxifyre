// pch.h: This is a precompiled header file.
// Files listed below are compiled only once, improving build performance for future builds.
// This also affects IntelliSense performance, including code completion and many code browsing features.
// However, files listed here are ALL re-compiled if any one of them is updated between builds.
// Do not add files here that you will be updating frequently as this negates the performance advantage.

#ifndef PCH_H
#define PCH_H

#include <string>
#include <memory>
#include <variant>
#include <vector>
#define NOMINMAX 1

#include <WinSock2.h>
#include <ws2tcpip.h>
#include <in6addr.h>
#include <tchar.h>
#include <ws2ipdef.h>
#include <IPHlpApi.h>
#include <Mstcpip.h>
#include <WinDNS.h>
#include <conio.h>
#include <stdlib.h>
#include <vector>
#include <array>
#include <map>
#include <set>
#include <optional>
#include <functional>
#include <bitset>
#include <variant>
#include <algorithm>
#include <mutex>
#include <shared_mutex>
#include <iostream>
#include <fstream>
#include <stack>
#include <charconv>
#include <unordered_set>
#include <queue>
#include <regex>
#include <gsl/gsl>

#include "../include/common.h"
#include "../include/ndisapi.h"
#include "../netlib/log/log.h"
#include "../netlib/tools/generic.h"
#include "../netlib/iphlp.h"
#include "../netlib/winsys/object.h"
#include "../netlib/winsys/event.h"
#include "../netlib/winsys/io_completion_port.h"
#include "../netlib/net/mac_address.h"
#include "../netlib/net/ip_address.h"
#include "../netlib/net/ip_subnet.h"
#include "../netlib/net/ip_endpoint.h"
#include "../netlib/net/ipv6_helper.h"
#include "../netlib/pcap/pcap.h"
#include "../netlib/iphelper/network_adapter_info.h"
#include "../netlib/ndisapi/network_adapter.h"
#include "../netlib/ndisapi/queued_packet_filter.h"
#include "../netlib/ndisapi/static_filters.h"
#include "../netlib/pcap/pcap.h"
#include "../netlib/pcap/pcap_file_storage.h"
#include "../netlib/ndisapi/tcp_local_redirect.h"
#include "../netlib/proxy/proxy_common.h"
#include "../netlib/proxy/socks5_common.h"
#include "../netlib/ndisapi/socks5_udp_local_redirect.h"
#include "../netlib/winsys/io_completion_port.h"
#include "../netlib/proxy/packet_pool.h"
#include "../netlib/proxy/tcp_proxy_socket.h"
#include "../netlib/proxy/socks5_tcp_proxy_socket.h"
#include "../netlib/proxy/tcp_proxy_server.h"
#include "../netlib/proxy/socks5_udp_proxy_socket.h"
#include "../netlib/proxy/socks5_local_udp_proxy_server.h"
#include "../netlib/iphelper/network_adapter_info.h"
#include "../netlib/iphelper/process_lookup.h"
#include "../netlib/proxy/socks_local_router.h"
#include "json.hpp"
enum class log_level_mx
{
	none = 0,
	info = 1,
	deb = 2,
	all = 3,
};

enum class status_mx
{
	stopped,
	connected,
	disconnected,
	error
};

enum class supported_protocols_mx
{
	tcp,
	udp,
	both
};

enum class event_type_mx : uint32_t
{
	connected,
	disconnected,
	address_error,
	normal = 999,

};


struct event_mx
{
	long long time; // event time
	event_type_mx type; // event type
	size_t data; // optional data
	std::string msg;
};


using log_storage_mx_t = std::vector<event_mx>;

#endif //PCH_H
