myApp
    .factory('locationCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchLocationByCompany = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/S0004'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }            

            fac.FetchWarehouseByLoggedLocationAndCompany = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0053'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            fac.FetchWarehouseBySelectedLocation = function (locationId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0053E'
                        + '?locationId=' + locationId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.FetchLocationByCompanyExceptOwnLocation = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/S0005'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            return fac;
        }
    ]);