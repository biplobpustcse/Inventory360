myApp
    .factory('convertionRatioSetupService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchConvertionRatioLists = function (q, pageIndex, pageSize) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/S204'
                        + '?query=' + q
                        + '&pageIndex=' + pageIndex
                        + '&pageSize=' + pageSize
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            };            
            fac.SaveOrUpdateConvertionRatioSetup = function (convertionRatio, lblSave) {
                if (lblSave == "Save") {
                    return $http({
                        method: 'POST',
                        url: serviceBasePath + '/api/SI202',
                        dataType: 'json',
                        data: JSON.stringify(convertionRatio),
                        headers: { "Content-Type": "application/json" }
                    }).then(function (response) {
                        return response;
                    }, function (error) {
                        return error;
                    });
                }
                else if (lblSave == "Update") {
                    return $http({
                        method: 'POST',
                        url: serviceBasePath + '/api/SU202',
                        dataType: 'json',
                        data: JSON.stringify(convertionRatio),
                        headers: { "Content-Type": "application/json" }
                    }).then(function (response) {
                        return response;
                    }, function (error) {
                        return error;
                    });
                }
            };

            return fac;
        }
    ]);