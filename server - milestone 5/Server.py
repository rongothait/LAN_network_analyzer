import sys, socket, subprocess
import threading
from Computer import *

orig_stdout = sys.stdout  # to make print show on the screen correctly
from scapy.all import *

sys.stdout = orig_stdout  # stdout --> return to original
import socket
from socket import inet_aton
import struct

# variable declaration:
server_ip = socket.gethostbyname(socket.gethostname())  # the server's ip address
computers_arr = []  # a list of all the computers
computers_ip_arr = []  # a list of the computer's IP's
network_ip_arr = server_ip.split(".")[0:3]  # get the network's IP adress (example - 172.16.15.0)
global admin_sock
admin_sock = None


# functions declaration:
def get_open_port():
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.bind(("", 0))
    s.listen(1)
    port = s.getsockname()[1]
    s.close()
    print "port: ", port
    print "ip: ", server_ip
    return port


def get_network_computers():
    for x in range(0, 256):
        if not x == int(server_ip.split(".")[3]):  # do not send arp message to yourself
            sendp(Ether(dst="FF:FF:FF:FF:FF:FF") / ARP(op=ARP.who_has, psrc=server_ip,
                                                       pdst='{0}.{1}.{2}.{3}'.format(network_ip_arr[0],
                                                                                     network_ip_arr[1],
                                                                                     network_ip_arr[2], x)), verbose=0)


def arp_sniff():
    print 'started sniffing!'
    sniff(prn=analyze_packet, filter="arp")


def analyze_packet(pkt):
    global computers_arr
    global computers_ip_arr
    if pkt.op == 2:
        if ('{0}.{1}.{2}'.format(network_ip_arr[0], network_ip_arr[1], network_ip_arr[2]) in pkt[ARP].psrc) and (
            '{0}.{1}.{2}'.format(network_ip_arr[0], network_ip_arr[1], network_ip_arr[2]) in pkt[ARP].pdst) and (
            server_ip not in pkt[ARP].psrc):
            # print pkt.show()
            if pkt[ARP].psrc not in computers_ip_arr:  # if this computer didn't connect through socket
                tmp_comp = Computer(pkt[ARP].psrc, pkt[ARP].hwsrc)
                computers_arr.append(tmp_comp)
                computers_ip_arr.append(pkt[ARP].psrc)
                t = threading.Thread(target=tmp_comp.OS_FingerPrinting, args=())
                t.start()


def listener():
    server_sock.listen(5)
    while 1:
        print "waiting for connections..."
        (clientsock, address) = server_sock.accept()
        print "address {0} is now connected".format(address)
        t = threading.Thread(target=general_handler, args=(clientsock, address))
        t.start()


def general_handler(clisock, address):
    client_type = clisock.recv(1024)  # an admin or a regular client
    print 'client type: ', client_type
    if client_type == 'admin':
        admin_handler(clisock, address)
    elif client_type == 'normal client':
        normal_client_handler(clisock, address)
    else:
        print 'Error: No client type found for - ', client_type


def admin_handler(clisock, address):
    global admin_sock
    admin_sock = clisock

    while 1:
        try:
            instruction_msg = clisock.recv(1024)
        except:
            continue
        if instruction_msg == 'network status':
            clisock.send(("network status-" + make_network_status_str() + '*'))

        elif 'info and network status-' in instruction_msg:
            try:
                admin_sock.send('info and network status-{0}&{1}*'.format(make_network_status_str(), make_computer_info(instruction_msg.split('-')[1])))
            except:
                admin_sock.send("computer disconnected")

            for comp in computers_arr:
                if comp.IP == instruction_msg.split('-')[1]:
                    if comp.clisock is not None:
                        comp.clisock.send('ports domain request*')


        elif 'illegal list update' in instruction_msg:
            tmp_ip = instruction_msg.split('-')[1]
            tmp_app = instruction_msg.split('-')[2]
            for comp in computers_arr:
                if comp.IP == tmp_ip:
                    if comp.has_client:
                        comp.app_illegal_list.append(tmp_app)
                        print "appended illegal list with {0} for {1}".format(tmp_app, comp.IP)
                        print comp.app_illegal_list
                        comp.clisock.send('illegal list update-' + ','.join(comp.app_illegal_list) + '*')
                        print "sent to client about change :)"


        elif 'illegal domain update-' in instruction_msg:
            tmp_ip = instruction_msg.split('-')[1]
            tmp_domain = instruction_msg.split('-')[2]
            for comp in computers_arr:
                if comp.IP == tmp_ip:
                    if comp.has_client:
                        comp.domain_illegal_list.append(tmp_domain)
                    print "appended domain list with {0} for {1}".format(tmp_domain, tmp_ip)



def normal_client_handler(clisock, address):
    global computers_arr
    global computers_ip_arr
    client_os = clisock.recv(1024)
    # add computer to list of computers!
    t = threading.Thread(target=check_for_client_mac, args=(address[0], ))
    t.start()

    if address[0] in computers_ip_arr:  # if computer was found via ARP
        for comp in computers_arr:
            if comp.IP == address[0]:
                comp.has_client = True
                comp.clisock = clisock
                comp.OS = client_os

    else:  # if computer wasn't found via ARP
        tmp_cmp = Computer(address[0], "not found")
        tmp_cmp.has_client = True
        tmp_cmp.clisock = clisock
        tmp_cmp.OS = client_os
        computers_arr.append(tmp_cmp)
        computers_ip_arr.append(address[0])

    # wait for notification about illegal apps!
    while 1:
        try:
            illegal_notification = clisock.recv(1024)

        except:
            for comp in computers_arr:
                if comp.IP == address[0]:
                    comp.app_illegal_list = []
                    comp.domain_illegal_list = []
                    comp.notified_domain = ''
                    comp.has_client = False
                    comp.clisock = None
                    comp.telnet_list = ''
                    comp.https_list = ''
                    comp.http_list = ''
                    comp.Domain = ''
                    print 'client disconnected!'
            break

        if "illegal notification-" in illegal_notification:
            illegal_notification = illegal_notification.split('-')[1]
            print 'user {0} entered {1}... notifying admin!'.format(address[0], illegal_notification)
            try:
                admin_sock.send("illegal notification-{0}-{1}".format(address[0], illegal_notification))
            except:
                print "Admin disconnected... cant notify"

        elif "ports domain status" in illegal_notification:
            msg = illegal_notification.split('-')[1].split('#')
            for comp in computers_arr:
                if comp.IP == address[0]:
                    comp.http_list = msg[0]
                    comp.https_list = msg[1]
                    comp.telnet_list = msg[2]
                    comp.Domain = msg[3]
                    check_if_illegal_domain(address[0], msg[3])


def check_if_illegal_domain(ip, domain):
    if domain != '':

        for comp in computers_arr:
            if comp.IP == ip:
                if domain.find(comp.notified_domain):
                    comp.notified_domain = ''

        for comp in computers_arr:
            if comp.IP == ip:
                for illegal_domain in comp.domain_illegal_list:
                    if (domain.find(illegal_domain) != -1) and (illegal_domain != comp.notified_domain):  # if the domain is illegal and hasn't yet been notified
                        try:
                            admin_sock.send("illegal notification-{0}-{1}".format(ip, illegal_domain))
                        except:
                            print "Admin disconnected... cant notify"
                        comp.notified_domain = illegal_domain  # notified domain is now this domain




def check_for_client_mac(ip):
    print "checking for mac address"
    response = srp1(Ether(dst="FF:FF:FF:FF:FF:FF")/ARP(op=ARP.who_has, psrc=server_ip, pdst=ip), verbose=0)
    try:
        mac = response[ARP].hwsrc
        for comp in computers_arr:
            if comp.IP == ip:
                comp.MAC = mac
    except:
        pass


def make_network_status_str():
    network_status = ''
    tmp_ip_lst = computers_ip_arr
    tmp_ip_lst = sorted(tmp_ip_lst, key = lambda ip: struct.unpack("!L", inet_aton(ip))[0])

    for ip in tmp_ip_lst:
        if network_status == '':
            for computer in computers_arr:
                if computer.IP == ip:
                    if computer.has_client:
                        network_status += '{0}+'.format(computer.IP)
                    else:
                        network_status += '{0}'.format(computer.IP)

        else:  # if network status is not empty
            for computer in computers_arr:
                if computer.IP == ip:
                    if computer.has_client:
                        network_status += '@{0}+'.format(computer.IP)
                    else:
                        network_status += '@{0}'.format(computer.IP)

    return network_status

def make_computer_info(ip):
    for comp in computers_arr:
        if comp.IP == ip:
            msg = '{0}@{1}@{2}@{3}@{4}@{5}@{6}@{7}@{8}@{9}'.format(comp.IP, comp.MAC, comp.OS, ', '.join(comp.app_illegal_list),
                                                           comp.Domain, comp.http_list,
                                                           comp.https_list, comp.telnet_list, ', '.join(comp.domain_illegal_list), comp.has_client)
            return msg
    raise ValueError('I know what happend...')





server_sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_sock.bind((server_ip, get_open_port()))
t = threading.Thread(target=listener, args=())
t.start()
y = threading.Thread(target = arp_sniff)
y.start()
get_network_computers()

