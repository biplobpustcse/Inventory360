myApp
    .factory('collectionAgainstCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchCollectionAgainst = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0066'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);