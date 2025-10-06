using System.Linq;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayManager : MonoBehaviour
{
    public static RelayManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    
    public async Task<string> StartHostWithRelay()
    {
        //create an allocation for only one other peer; default region
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);
        //get allocation join code
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        //set up transport
        RelayServerData relayServerData = AllocationUtils.ToRelayServerData(allocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartHost(); //start host

        return joinCode;
    }

    /*
     * 
     */

    public async Task<bool> StartClientWithRelay(string joinCode)
    {
        //join allocation with join code
        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        //set up transport
        RelayServerData relayServerData = AllocationUtils.ToRelayServerData(joinAllocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartClient(); //start client

        return true;
    }

    /*
    public async Task<string> CreateRelay()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, "dtls"));
        NetworkManager.Singleton.StartHost();

        return joinCode;
    }
    */
}
