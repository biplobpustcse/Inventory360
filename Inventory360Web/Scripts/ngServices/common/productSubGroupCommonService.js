myApp
    .factory('productSubGroupCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchProductSubGroup = function (q, productGroupId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0027'
                        + '?query=' + q
                        + '&productGroupId=' + productGroupId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            }

            return fac;
        }
    ]);