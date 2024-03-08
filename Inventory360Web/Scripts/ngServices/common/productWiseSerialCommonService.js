myApp
    .factory('productWiseSerialCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchSerialByProduct = function (q, productId, dimensionId, unitTypeId, warehouseId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0072'
                        + '?query=' + q
                        + '&productId=' + productId
                        + '&dimensionId=' + dimensionId
                        + '&unitTypeId=' + unitTypeId
                        + '&warehouseId=' + warehouseId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            fac.FetchAllSerialByProduct = function (q, productId, dimensionId, unitTypeId, warehouseId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0075'
                        + '?query=' + q
                        + '&productId=' + productId
                        + '&dimensionId=' + dimensionId
                        + '&unitTypeId=' + unitTypeId
                        + '&warehouseId=' + warehouseId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);