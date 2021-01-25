# MSFS2020_Wrapper
Duplicates Xplane's UDP-addressible interface for MSFS 2020.  Compile as part of MSFS 2020 SDK.

To build and run:
1.  Install Microsoft Flight Simulator 2020
2.  Install Visual Studio 2019 (Community version works fine)
3.  Start MSFS2020 and follow this guide to install the MSFS 2020 SDK:  https://forums.flightsimulator.com/t/how-to-getting-started-with-the-sdk-dev-mode/123241
4.  Git Clone or download and unzip this repositoy in to C:/MSFS SDK/Samples/MSFS2020_Wrapper
5.  Open the .sln file in VS2019, build the SimvarWatcher app.
6.  With MSFS 2020 running, start the app and click "connect".  You should see the indicator in the UI switch to green indicating a successful connection.
7.  Now, start the sim on the host computer and either toggle MSFS rendering in QGroundControl or via kXplaneHost in the xwing config launch script.
