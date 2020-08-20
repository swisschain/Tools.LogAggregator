# Tools.LogAggregator

![Validate master](https://github.com/swisschain/Tools.LogAggregator/workflows/Validate%20master/badge.svg)

![Release Service](https://github.com/swisschain/Tools.LogAggregator/workflows/Release%20Service/badge.svg)

![API docker image: swisschains/tools-log-aggregator](https://img.shields.io/docker/v/swisschains/tools-log-aggregator?sort=semver&label=swisschains/tools-log-aggregator)

**Docker image:** [swisschains/tools-log-aggregator](https://hub.docker.com/repository/docker/swisschains/tools-log-aggregator/tags?page=1)

# Environment Variables

| variable | value |
| -------- | ----- |
| ElasticSearchSettings__Url | url address to ELK api (http://elasticsearch.elk-logs.svc.cluster.local:9200) |

# ports

| port | description |
| ---- | ----------- |
| 5000 | HTTP Rest API. `/swagger/ui` |
| 5001 | gRPC [proto](https://github.com/swisschain/Tools.LogAggregator/tree/master/src/LogAggregator.ApiContract) |

# Kybernates

* [Deployment.yaml](https://github.com/swisschain/Tools.LogAggregator/tree/master/deployment/kubernetes/Service-LogAggreggator)
* [Service.yaml](https://github.com/swisschain/Tools.LogAggregator/tree/master/deployment/kubernetes/Service-LogAggreggator)
