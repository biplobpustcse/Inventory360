myApp
    .controller('collectionReportController', ['$scope', 'dataTableRows', 'applicationBasePath', 'userService', 'currencyCommonService', 'allCollectionService',
        function (
            $scope,
            dataTableRows,
            applicationBasePath,
            userService,
            currencyCommonService,
            allCollectionService
        ) {
            $scope.pageIndex = 1;
            $scope.showRecordsInfo = '';
            $scope.showGrid = false;
            $scope.allCollectionJsonLists = [];

            $scope.previous = function (e) {
                e.preventDefault();
                if ($scope.pageIndex > 1) {
                    $scope.pageIndex--;
                    GetAllCollectionGridListsData();
                }
            };

            $scope.next = function (e) {
                e.preventDefault();
                if ($scope.pageIndex < lastPageNo) {
                    $scope.pageIndex++;
                    GetAllCollectionGridListsData();
                }
            };

            $scope.filterCollectionNo = function () {
                if ($scope.searchCollectionNo !== undefined) {
                    $scope.pageIndex = 1;
                    GetAllCollectionGridListsData();
                }
            };

            $scope.loadCollectionNoByCurrency = function () {
                GetAllCollectionGridListsData();
            };

            $scope.PrintReport = function (id) {
                var url = applicationBasePath + "/Inventory360Reports/PrintIndCollection?id=" + id + "&user=" + $scope.UserName + "&currency=" + $scope.currencyId + "&token=" + userService.GetCurrentUser().access_token;
                var win = window.open(url, '_blank');
                win.focus();
            };

            // load currency
            var GetCurrencyForDropdown = function () {
                currencyCommonService.FetchCurrency().then(function (value) {
                    $scope.currencyDropDownJsonData = value.data;
                    $scope.currencyId = $scope.DefaultCurrency;
                    GetAllCollectionGridListsData();
                }, function (reason) {
                    console.log(reason);
                });
            }

            // load all collection lists
            var GetAllCollectionGridListsData = function () {
                var q = $scope.searchCollectionNo === undefined ? '' : $scope.searchCollectionNo;

                allCollectionService.FetchAllCollectionLists(
                    q, $scope.currencyId, $scope.pageIndex, dataTableRows
                ).then(function (value) {
                    $scope.allCollectionJsonLists = value.data;
                    lastPageNo = value.data.LastPageNo;
                    if (value.data.TotalNumberOfRecords > 0) {
                        $scope.showGrid = true;
                        $scope.showRecordsInfo = 'Record(s) showing ' + value.data.Start + " to " + value.data.End + " of " + value.data.TotalNumberOfRecords;
                    } else {
                        $scope.showGrid = false;
                        $scope.showRecordsInfo = "No record(s) found...";
                    }
                }, function (reason) {
                    console.log(reason);
                });
            }

            GetCurrencyForDropdown();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });