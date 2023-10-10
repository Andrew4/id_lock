
# Introduction

This project offers a consensus algorithm.
It helps build a detailed data layer of an application cluster.

I packaged the algorithm and its accessories in a program that runs on `Linux`.
If you need the algorithm on a microcontroller, contact me.

---

## Project tester

You can find the **id-test** in the `tester` folder.
I use that `C#` console application to simulate data flow that tests the **id-lock** for bugs and performance.
You can try it on your computer if you build a compatible testing environment with mine.

- I am using `Windows 10`.
- Install `WSL 2` https://learn.microsoft.com/en-us/windows/wsl/install
- Accept the default `Ubuntu` image.
- Install `WSL` kernel upgrade https://github.com/microsoft/WSL2-Linux-Kernel/releases
- Install `PowerShell` https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.3
- Open an elevated `PowerShell` console, and create a new firewall rule for `WSL 2`
```bash
New-NetFirewallRule -Name 'WSL_IN' -DisplayName 'WSL_IN' -InterfaceAlias 'vEthernet (WSL)' -Direction Inbound -Action Allow
```
- Other useful commands (remove & check):
```bash
Remove-NetFirewallRule -DisplayName 'WSL_IN'
Get-NetFirewallRule -DisplayName 'WSL_IN'
```

- You can find your local drives on the `WSL` console here
```bash
cd /mnt
ls -la
```
- Go into the `build` folder of the project and start the **id-lock** three times:
```bash
./id-lock-0.0.2 c501 &
./id-lock-0.0.2 c502 &
./id-lock-0.0.2 c503 &
```
- Figure out the local IP of the `WSL 2` guest:
```bash
ip r
```
- Find the `win_com.wsl_remote_ip=` part in the `tester/win_com.cs` source.
Update the IP address there.
- Run the `compiler.bat` script.
- Start the `id-test.exe` three times.
- Read this page if a firewall problem arises: https://support.microsoft.com/en-us/windows/risks-of-allowing-apps-through-windows-defender-firewall-654559af-3f54-3dcf-349f-71ccd90bcc5c
- The **id-lock** and **id-test** instances will pair up automatically.
The protocol has its starting phases.
A running tester looks like the `GIF` video below (version 0.0.1):
![screen-capture](https://github.com/Andrew4/id_lock/assets/6005439/6f63f106-31c2-4e6a-a45d-f59ab0ad3b2d)

The `[error_flags: 0x000000]` field in the first line reports errors.
The lowest byte indicates errors in the algorithm.
The middle byte indicates warnings or errors in the **id-test** application.
Please send me feedback if any other value than `0x000000` shows up.

There are built-in parameters in the source code of the **id-test**.
Those parameters set the default performance to low.
An old low-performance laptop can run it smoothly.

