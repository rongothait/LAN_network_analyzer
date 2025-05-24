from scapy.all import*


class Computer:
    OS = ''
    IP = ''
    MAC = ''



    def __init__(self, IP, MAC):
        self.IP = IP
        self.MAC = MAC
        self.app_illegal_list = []
        self.domain_illegal_list = []
        self.notified_domain = ''
        self.http_list = ''
        self.https_list = ''
        self.telnet_list = ''
        self.has_client = False
        self.OS = ''
        self.clisock = None
        self.Domain = ''
        #self.disconnected = False

    def OS_FingerPrinting(self):
        print "Finding os for - {0}".format(self.IP)
        first_pkt = sr1(IP(dst=self.IP) / ICMP(type=8, code=8), timeout = 15, verbose = 0)
        if first_pkt is not None:
            if first_pkt[ICMP].code == 0:
                self.OS = "Windows"

            else:
                other_first_pkt = sr1(IP(dst=self.IP) / ICMP(type=13, code=0), timeout=15, verbose=0)
                if other_first_pkt is not None:
                    self.OS = "Linux"

                else:
                    self.OS = "Other OS"

        else:
            self.OS = "not found!"



        print '\033[1;42m{0}\033[1;m'.format('{0} OS is {1}'.format(self.IP, self.OS))