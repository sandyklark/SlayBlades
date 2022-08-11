using System;
using UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Networking
{
    [RequireComponent(typeof(NetworkTransport))]
    public class RelayInitialisation : MonoBehaviour
    {
        public NetworkUI networkUI;

        private struct RelayHostData
        {
            public string JoinCode;
            public string IPv4Address;
            public ushort Port;
            public Guid AllocationID;
            public byte[] AllocationIDBytes;
            public byte[] ConnectionData;
            public byte[] Key;
        }

        private struct RelayJoinData
        {
            public string IPv4Address;
            public ushort Port;
            public Guid AllocationID;
            public byte[] AllocationIDBytes;
            public byte[] ConnectionData;
            public byte[] HostConnectionData;
            public byte[] Key;
        }

        public async void HostGame(int maxConn)
        {
            //Initialize the Unity Services engine
            Debug.Log("Initialising Unity Services: STARTING");
            await UnityServices.InitializeAsync();
            Debug.Log("Initialising Unity Services: DONE");

            //Always authenticate your users beforehand
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                Debug.Log("Not Logged in");
                Debug.Log("Logging in to Auth Service: STARTING");
                //If not already logged, log the user in
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Logging in to Auth Service: DONE");
            }
            else
            {
                Debug.Log("Already Logged in");
            }

            //Ask Unity Services to allocate a Relay server
            Debug.Log("Requesting Relay Allocation: STARTING");
            var allocation = await Unity.Services.Relay.RelayService.Instance.CreateAllocationAsync(maxConn);
            Debug.Log("Requesting Relay Allocation: DONE");

            //Populate the hosting data
            var data = new RelayHostData
            {
                // WARNING allocation.RelayServer is deprecated
                IPv4Address = allocation.RelayServer.IpV4,
                Port = (ushort)allocation.RelayServer.Port,
                AllocationID = allocation.AllocationId,
                AllocationIDBytes = allocation.AllocationIdBytes,
                ConnectionData = allocation.ConnectionData,
                Key = allocation.Key,
            };

            //Retrieve the Relay join code for our clients to join our party
            data.JoinCode = await Unity.Services.Relay.RelayService.Instance.GetJoinCodeAsync(data.AllocationID);


            Debug.Log("Join Code: " + data.JoinCode);
            NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>().SetHostRelayData(
                data.IPv4Address,
                data.Port,
                data.AllocationIDBytes,
                data.Key,
                data.ConnectionData
            );

            // Start NGo Server
            NetworkManager.Singleton.StartHost();

            networkUI.Hide();
        }

        public async void JoinGame()
        {
            // Get join code string
            var joinCode = networkUI.GetJoinCodeText();
            Debug.Log($"Got join code: {joinCode}");

            //Initialize the Unity Services engine
            await UnityServices.InitializeAsync();

            //Always authenticate your users beforehand
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                //If not already logged, log the user in
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            //Ask Unity Services for allocation data based on a join code
            var allocation = await Unity.Services.Relay.RelayService.Instance.JoinAllocationAsync(joinCode);

            //Populate the joining data
            var data = new RelayJoinData
            {
                // WARNING allocation.RelayServer is deprecated. It's best to read from ServerEndpoints.
                IPv4Address = allocation.RelayServer.IpV4,
                Port = (ushort)allocation.RelayServer.Port,
                AllocationID = allocation.AllocationId,
                AllocationIDBytes = allocation.AllocationIdBytes,
                ConnectionData = allocation.ConnectionData,
                HostConnectionData = allocation.HostConnectionData,
                Key = allocation.Key,
            };

            Debug.Log($"Joining: {joinCode}");
            NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>().SetClientRelayData(
                data.IPv4Address,
                data.Port,
                data.AllocationIDBytes,
                data.Key,
                data.ConnectionData,
                data.HostConnectionData
            );

            // Start NGo Client
            NetworkManager.Singleton.StartClient();

            networkUI.Hide();
        }
    }
}
