using CookComputing.XmlRpc;

namespace InwxClient {

    public struct LoginParameter {
        public string lang;
        public string user;
        public string pass;
        public LoginParameter(string user,string pass) {
            this.user = user; this.pass = pass; this.lang = "en";
        }
    }
    public struct InwxResult<ResDataType> {
        public int code;
        public string msg;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string reasonCode;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string reason;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public ResDataType resData;
        public string svTRID;

        public override string ToString() {
            string x = "";
            x += "code: " + code.ToString() + "\n";
            x += "msg: " + msg + "\n";
            x += "reasonCode: " + reasonCode + "\n";
            x += "reason: " + reason + "\n";
            x += "resData: ";
            if (resData == null) x += "<NULL>";
            else if (resData is XmlRpcStruct) x += "{\n" + Program.RpcStructToString((XmlRpcStruct)(object)resData) + "}";
            else x += resData.ToString();
            x += "\n";
            x += "svTRID: " + svTRID + "\n";
            return x;
        }
    }
    public struct LoginResData {
        public int customerId;
        public int accountId;
        public string tfa;
        public string builddate;
        public string version;
    }

    public struct UnlockParameter {
        public string tan;
        public UnlockParameter(string tan) {
            this.tan = tan;
        }
    }


    public struct NameserverInfoParameter {
        public string domain;
        public NameserverInfoParameter(string domain) {
            this.domain = domain;
        }
    }
    public struct NameserverInfoResData {
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public int roId;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string domain;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string type; //MASTER
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public object masterIp;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public object lastZoneCheck;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public object slaveDns;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string SOAserial;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public int count;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public NameserverRecord[] record;
    }
    public struct NameserverRecord {
        public int id;
        public string name;
        public string type;
        public string content;
        public int ttl;
        public int prio;
    }


    public struct NameserverCreateRecord {
        public string domain;
        public string type;
        public string content;
        public string name;
        public int ttl;
        public int prio;
    }

    

    public struct NameserverListResData {
        public int count;
        public NameserverListItem[] domains;
    }
    public struct NameserverListItem {
        public int roId;
        public string domain;
        public string type;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string masterIp;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string mail;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string web;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string url;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string ipv4;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string ipv6;
    }

    [XmlRpcUrl("https://api.domrobot.com/xmlrpc/")]
    public interface IInwxClient : IXmlRpcProxy {
        [XmlRpcMethod("account.login")]
        InwxResult<LoginResData> login(LoginParameter param);

        [XmlRpcMethod("account.unlock")]
        InwxResult<XmlRpcStruct> login_unlock(UnlockParameter param);

        [XmlRpcMethod("account.info")]
        InwxResult<XmlRpcStruct> account_info();

        [XmlRpcMethod("nameserver.info")]
        InwxResult<NameserverInfoResData> nameserver_info(NameserverInfoParameter param);

        [XmlRpcMethod("nameserver.updateRecord")]
        InwxResult<XmlRpcStruct> nameserver_updateRecord(NameserverRecord param);

        [XmlRpcMethod("nameserver.createRecord")]
        InwxResult<XmlRpcStruct> nameserver_createRecord(NameserverCreateRecord param);

        [XmlRpcMethod("nameserver.list")]
        InwxResult<NameserverListResData> nameserver_list();



    }
}
