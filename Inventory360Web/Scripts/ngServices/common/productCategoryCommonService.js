myApp
    .factory('productCategoryCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchProductCategory = function (q, productGroupId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0028'
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