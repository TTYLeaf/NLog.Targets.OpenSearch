# NLog.Targets.OpenSearch

[![Build status](https://ci.appveyor.com/api/projects/status/53pvt1ao61hd3ym2/branch/master?svg=true)](https://ci.appveyor.com/project/markmcdowell/nlog-targets-elasticsearch/branch/master)
[![NuGet Pre Release](https://img.shields.io/nuget/vpre/NLog.Targets.ElasticSearch.svg)](https://www.nuget.org/packages/NLog.Targets.ElasticSearch)

The Elasticsearch target works best with the BufferingWrapper target applied. By default the target assumes an Elasticsearch node is running on the localhost on port 9200.

See [wiki](https://github.com/ReactiveMarkets/NLog.Targets.ElasticSearch/wiki) for parameters.

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
