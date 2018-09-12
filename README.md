# SpPerfMon
Monitoring tool for SharePoint Online

This is a simple utility for monitoring your SharePoint Online environment's performance and discovering issues as they arise. 
It keeps track of its metrics in a convenient .csv format and can send alerts when issues are discovered.

- [Setup](#setup)
  - [Get the utility](#getit)
  - [Specify configuration values](#config)
  - [Install as a Windows service](#service)
  - [Run temporarily without installing](#temporary)
- [Logging](#logging)
- [Alerts](#alerts)
- [Managing the Windows service](#manageservice)
  - [Changing configuration settings](#changesettings)
  - [Uninstalling the service](#uninstall)

## Setup

### <a id="getit"></a> Get the utility

You can download SpPerfMon from its [Github repository](https://github.com/Qdabra/SpPerfMon). Click **Clone or download** and then **Download ZIP**.

All of the relevant program files will be in the **dist** folder within the ZIP.

### <a name="config"></a> Specify configuration values

1. Open the SpPerfMon.exe.config file in any text editor.

2. Specify values for the following appSettings:

    a. **username** The SharePoint Online username that the tool should use for accessing your SharePoint Online environment.

    b. **password** The password for the above username

    c. **intervalSeconds** _(optional)_ The frequency at which the utility should query each SharePoint endpoint.

    d. **notificationRecipients** A list of e-mail addresses to which the utility should send alerts when performance issues are found. This should be one or more e-mail addresses, separated with commas. e.g. `person1@mycompany.com,person2@mycompany.com,person3@mycompany.com`

    e. **maxRequestSeconds** _(optional)_ The maximum number of seconds to consider as a "normal" request. Any total request time above this will be considered a potential performance issue.

    f. **maxRequestDurationMilliseconds** _(optional)_ The maximum SPRequestDuration (as reported by SharePoint) to consider as "normal". Anything above this will be considered a potential performance issue.

    g. **maxSharePointHealth** _(optional)_ The maximum SPHealthScore (as reported by SharePoint) to consider as "normal". Anything above this will be considered a potential performance issue.

3. Specify SharePoint endpoints - In the `<endpoints>` section, specify as many SharePoint Online endpoints that you would like to monitor. It is recommended that you specify at least two - one site that you use on a regular basis, and one site in an empty site collection. Each endpoint should have a unique `name` consisting only of letters, numbers, and spaces, and a `url`.

4. Specify SMTP settings - specify SMTP values in accordance with the comments in the config file. These settings will be used for sending notifications when potential performance issues are detected.

### <a name="service"></a> (Recommended) Install SpPerfMon as a Windows service

It is recommended that you install SpPerfMon as a Windows service so that it can operate uninterrupted at all hours of the day.

1. Place all of the program files, including your edited .config file, in an empty folder on a server where you have Administrator access. The files should remain there for as long as you are using the utility. If you don't have a server to use for this, you can use a local machine, but the utility will only be able to operate while the machine is running.
2. Right-click the **install.bat** file and click **Run as administrator**
3. If prompted for elevated permissions, click Yes.
4. The install.bat file should install and start the service, and you should see text indicating _"Service "Qdabra SharePoint Performance Monitor" installed successfully!"_ and _"Qdabra SharePoint Performance Monitor: START: The operation completed successfully."_
5. The utility should now be installed as a service on your server (or local machine if that's what you used). See the **Managing the service** section of this README for more details about managing this service.

### <a id="temporary"></a> Run SpPerfMon temporarily without installing it

You can run SpPerfMon temporarily without installing it as a Windows service if you only want to monitor SharePoint for a short period of time. While it is running, it should write logs to the Logs folder and send alerts in the same way it would if it were installed as a service.

To do this, simply navigate to the program folder after you have prepared the necessary .config settings, and double click the SpPerfMon.exe file. This should open a console window and the tool should begin monitoring SharePoint.

You can terminate the program at any time by clicking the red X in the top-right of the console window.

## Logging

SpPerfMon will create a **Logs** folder in its program folder and place logs there.

Log files are named according to the date when the log event occurred. A log file named `[date] [endpoint name].csv` is created for each endpoint, and a file named `[date].csv` contains the combined logs for all endpoints. 

These files are in a .csv format that can be opened in Excel or any application that can consume CSV files. They contain the following columns:

- **Url** - The URL that was queried
- **Request Start** - The date and time that the query began
- **Request End** - The date and time that the query completed
- **Request Time Elapsed** - The total time (in seconds) between Request Start and Request End
- **Outcome** - The text "success" if the request completed successfully. An error message if the request failed.
- **X-SharePointHealthScore** - The SharePoint Health Score, as reported by SharePoint. This is a number between 0 and 10, with lower numbers indicating good health, and higher numbers indicating a potential issue.
- **SPRequestDuration** - The SPRequestDuration value reported by SharePoint. This is a value in milliseconds that essentially indicates the total server-side processing time to serve the request.
- **SPIisLatency** - The IIS Latency, as reported by SharePoint. Like SharePointHealthScore, this is another indicator of the SharePoint service's health, and should be 0 under normal circumstances.
- **SPRequestGuid** - A unique identifier for each request, as reported by SharePoint.

Any errors that SpPerfMon (aside from request failures) will be placed in a file named `[date] Errors.txt`.

You can freely delete any log files that you no longer need.

# Alerts

SpPerfMon will send an e-mail alert if it detects issues on two separate requests on a single endpoint within a five minute window.

Issues that can trigger an alert consist of:

- Total request times exceeding the **maxRequestSeconds** setting
- SPRequestDuration values exceeding the **maxRequestDurationMilliseconds** setting
- SharePointHealthScore values exceeding the **maxSharePointHealth** setting
- Any failed request (timeout, 404 error, etc.)

The alert e-mail will contain details about the two issues that triggered the alert.

After sending an alert for a given endpoint, SpPerfMon will wait five minutes before sending any more alerts for that endpoint.

# <a id="manageservice"></a> Managing the Windows Service

SpPerfMon should be installed as a Windows service with the name "Qdabra SharePoint Performance Monitor"

You can use the **Services** Control Panel app to stop and start this service, and to configure other service-related settings such as its service account and startup settings.

## <a id="changesettings"></a> Changing configuration settings

You can change configuration settings as needed by editing the program's .config file. You will need to restart the Windows service in order for the changes to take effect.

## <a id="uninstall"></a> Uninstalling the service

If you would like to remove SpPerfMon Windows service from your system, you can do so by using the included uninstall.bat file:

1. In SpPerfMon's program folder, right-click the **uninstall.bat** file and click **Run as administrator**
2. If you are prompted for elevated permissions, click **Yes**
3. You should be prompted with a confirmation dialog about removing the service. Click **Yes**
4. You should see a message indicating that the service was removed successfully
