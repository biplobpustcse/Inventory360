myApp
    .factory('problemSetupService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchProblemLists = function (q, pageIndex, pageSize) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/S201'
                        + '?query=' + q
                        + '&pageIndex=' + pageIndex
                        + '&pageSize=' + pageSize
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            };
            fac.FetchProblemForComplainReceive = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/S202'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.UpdateProblemSetup = function (problemId, eventName, subEventName, problemName) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/SU201',
                    dataType: 'json',
                    data: JSON.stringify({
                        "ProblemId": problemId,
                        "EventName": eventName,
                        "SubEventName": subEventName,
                        "Name": problemName
                    }),
                    headers: { "Content-Type": "application/json" }
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            };

            fac.SaveProblemSetup = function (eventName, subEventName, problemName) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/SI201',
                    dataType: 'json',
                    data: JSON.stringify({
                        "EventName": eventName,
                        "SubEventName": subEventName,
                        "Name": problemName
                    }),
                    headers: { "Content-Type": "application/json" }
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            };

            return fac;
        }
    ]);