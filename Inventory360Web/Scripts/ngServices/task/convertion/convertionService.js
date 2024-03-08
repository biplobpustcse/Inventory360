myApp
    .factory('convertionService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchConvertionLists = function (q, pageIndex, pageSize) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00219'
                        + '?query=' + q
                        + '&pageIndex=' + pageIndex
                        + '&pageSize=' + pageSize
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            };
            fac.FetchConvertionRatio = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/S205'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.FetchConvertionRatioById = function (convertionRatioId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/S206'
                        + '?convertionRatioId=' + convertionRatioId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.SaveConvertion = function (convertion) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TI00205',
                    dataType: 'json',
                    data: JSON.stringify(convertion),
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