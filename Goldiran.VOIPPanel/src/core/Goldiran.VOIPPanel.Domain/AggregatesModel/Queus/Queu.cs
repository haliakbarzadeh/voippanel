using Voip.Framework.Domain;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Queus;
public class Queu : AggregateRoot<int>
{
    public int Code { get; private set; }
    public string Name { get; private set; }
    public string IPAddress { get; private set; }
    public string User { get; private set; }
    public string Secret { get; private set; }
    public int Count {  get; private set; }
    public bool IsSLA { get; private set; }
    public bool IsFCR { get;private set; }

    public Queu(int code, string name, string iPAddress, string user, string secret, bool isSLA, bool isFCR)
    {
        Code = code;
        Name = name;
        Count = 0;
        IPAddress = iPAddress;
        User = user;
        Secret = secret;
        IsSLA = isSLA;
        IsFCR = isFCR;
    }
    public void Update(int code, string name, string ipAddress, string user, string secret, bool isSLA, bool isFCR)
    {
        Code = code;
        Name = name;
        IPAddress = ipAddress;
        User = user;
        Secret = secret;
        IsSLA = isSLA;
        IsFCR = isFCR;
    }

    public void Update(int code, string name, string ipAddress, string user, string secret, int count, bool isSLA, bool isFCR)
    {
        Code = code;
        Name = name;
        Count = count;
        IPAddress = ipAddress;
        User = user;
        Secret = secret;
        IsSLA = isSLA;
        IsFCR = isFCR;


    }

    public void ChangeCount(int count)
    {
        Count = count;
    }
}
