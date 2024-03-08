myApp
    .factory('productModelCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchProductModel = function (groupId, subGroupId, categoryId, brandId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0076'
                        + '?groupId=' + groupId
                        + '&subGroupId=' + subGroupId
                        + '&categoryId=' + categoryId
                        + '&brandId=' + brandId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            }

            return fac;
        }
    ]);