myApp
    .controller('convertionReportController', ['$scope', 'dataTableRows', 'applicationBasePath', 'userService', 'convertionService',
        function (
            $scope,
            dataTableRows,
            applicationBasePath,
            userService,
            convertionService
        ) {
            $scope.pageIndex = 1;
            $scope.showRecordsInfo = '';
            $scope.showGrid = false;
            $scope.convertionJsonLists = [];

            $scope.previous = function (e) {
                e.preventDefault();
                if ($scope.pageIndex > 1) {
                    $scope.pageIndex--;
                    AllConvertionGridListsData();
                }
            };

            $scope.next = function (e) {
                e.preventDefault();
                if ($scope.pageIndex < lastPageNo) {
                    $scope.pageIndex++;
                    AllConvertionGridListsData();
                }
            };

            $scope.filterConvertionNo = function () {
                if ($scope.searchConvertionNo !== undefined) {
                    $scope.pageIndex = 1;
                    AllConvertionGridListsData();
                }
            };
           
            $scope.loadData = function () {
                $scope.pageIndex = 1;
                AllConvertionGridListsData();
            };

            $scope.PrintReport = function (id) {
                var url = applicationBasePath + "/Inventory360Reports/PrintConvertion?id=" + id + "&user=" + $scope.UserName + "&currency=" + $scope.DefaultCurrency + "&token=" + userService.GetCurrentUser().access_token;
                var win = window.open(url, '_blank');
                win.focus();
            };

            // load all Convertion lists
            var AllConvertionGridListsData = function () {
                var q = $scope.searchConvertionNo === undefined ? '' : $scope.searchConvertionNo;

                convertionService.FetchConvertionLists(
                    q, $scope.pageIndex, dataTableRows
                ).then(function (value) {
                    $scope.convertionJsonLists = value.data;
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
            $scope.loadData();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });