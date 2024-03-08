myApp
    .factory('customerCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchAllCustomer = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0042'
                        + '?query=' + q
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.FetchOnlyBuyer = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0043'
                        + '?query=' + q
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.FetchAllCustomerByGroup = function (q, customerGroupId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00210'
                        + '?query=' + q
                        + '&customerGroupId=' + customerGroupId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            fac.FetchCustomerShortInfo = function (id) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/S0073'
                        + '?id=' + id
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.GetCustomerGroupAll = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00211'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);