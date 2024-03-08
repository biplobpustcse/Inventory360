myApp
    .controller('advanceSearchController', ['$scope', 'dataTableRows', 'applicationBasePath', 'userService', 'advanceSearchService',
        function (
            $scope,
            dataTableRows,
            applicationBasePath,
            userService,
            advanceSearchService
        ) {

            $scope.allCollectionJsonLists = [];

            $scope.PrintReport = function (id) {
                var url = applicationBasePath + "/Inventory360Reports/PrintIndCollection?id=" + id + "&user=" + $scope.UserName + "&currency=" + $scope.currencyId + "&token=" + userService.GetCurrentUser().access_token;
                var win = window.open(url, '_blank');
                win.focus();
            };

            // load all collection lists
            $scope.clickToSearch = function () {
                var q = $scope.serialNumber === undefined ? '' : $scope.serialNumber;
                if (q.length > 2) {
                    advanceSearchService.FetchAllAdvanceSearchDataLists(q).then(function (value) {
                        $scope.allCollectionJsonLists = value.data;
                    }, function (reason) {
                        console.log(reason);
                    });
                }
            }
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });