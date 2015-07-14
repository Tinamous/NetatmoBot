netatmo bot
-----------

Connects to Netatmo and reads weather information.

Consits or two main projects.
* NetatmoBot - a client for Netatmo
* NetatmoBot.ConsoleHost - a console host which currently looks for rain gauges within approx. 100km area of the centre points defined and see's how many stations are reporting rain.

This project is part of a bot for Tinamous that will allow users to import their own netatmo readings into their Tinamous account.

In order for this to work you should add your own "myAppSettings.config" file and set the parameters withint that file as specified in the App.config file.

To use this application you will need to create an application on Netatmo.com (http://dev.netatmo.com/dev/createapp)

<appSettings>
  <add key="Netatmo.ClientId" value="[Client Id from Natatmo application"/>
  <add key="Netatmo.ClientSecred" value="Client secret from Natatmo application"/>

  <add key="Netatmo.UserName" value="[your my.netatmo.com username (email)]"/>
  <add key="Netatmo.Password" value="[your password]"/>

  <add key="Latitude" value="[your location latitude as a number with decimal point e.g. 53.2343321]"/>
  <add key="Longitude" value="[your location longitude as a number with decimal point e.g. 0.1234545]"/>
</appSettings>


Build
-----

MSBuild Build.proj /p:Configuration=[Debug|Release]
MSBuild Build.proj /p:Configuration=[Debug|Release] /t:UnitTest
MSBuild Build.proj /p:Configuration=[Debug|Release] /t:IntegrationTest

MSBuild NuGetPack.proj /t:Package
MSBuild NuGetPack.proj /p:MyGetApiKey=[MyGet Api Key] /t:PublishToMyGet