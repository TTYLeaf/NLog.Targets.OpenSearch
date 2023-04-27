# NLog.Targets.OpenSearch

[![Build status](https://ci.appveyor.com/api/projects/status/53pvt1ao61hd3ym2/branch/master?svg=true)](https://ci.appveyor.com/project/markmcdowell/nlog-targets-elasticsearch/branch/master)
[![NuGet Pre Release](https://img.shields.io/nuget/vpre/NLog.Targets.ElasticSearch.svg)](https://www.nuget.org/packages/NLog.Targets.OpenSearch)

The OpenSearch target works best with the BufferingWrapper target applied.

See [wiki](https://github.com/ReactiveMarkets/NLog.Targets.OpenSearch/wiki) for parameters.

```xml
<nlog>
  <extensions>
    <add assembly="NLog.Targets.OpenSearch"/>
  </extensions>
  <targets>
    <target name="openSearch" xsi:type="BufferingWrapper" flushTimeout="5000">
      <target xsi:type="OpenSearch" uri="http://localhost:9200/" />
    </target>
  </targets>
  <rules>
    <logger name="*" minlevel="Info" writeTo="openSearch" />
  </rules>
</nlog>
```
