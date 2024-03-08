myApp
    .factory('termsAndConditionsCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchTermsAndConditions = function (id) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/S0072'
                        + '?id=' + id
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);