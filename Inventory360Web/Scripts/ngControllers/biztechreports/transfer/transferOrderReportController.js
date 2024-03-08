myApp
    .controller('transferOrderReportController', ['$scope', 'dataTableRows', 'applicationBasePath', 'userService',
        'allTransferOrderService', 'transferOrderWithDetailService',
        function (
            $scope, dataTableRows, applicationBasePath, userService,            
            allTransferOrderService, transferOrderWithDetailService
        ) {
            $scope.pageIndex = 1;
            $scope.showRecordsInfo = '';
            $scope.showGrid = false;
            $scope.alltransferOrderJsonLists = [];

            $scope.previous = function (e) {
                e.preventDefault();
                if ($scope.pageIndex > 1) {
                    $scope.pageIndex--;
                    GetAllTransferOrderGridListsData();
                }
            };

            $scope.next = function (e) {
                e.preventDefault();
                if ($scope.pageIndex < lastPageNo) {
                    $scope.pageIndex++;
                    GetAllTransferOrderGridListsData();
                }
            };

            $scope.filterTransferOrderNo = function () {
                if ($scope.searchTransferOrder !== undefined) {
                    $scope.pageIndex = 1;
                    GetAllTransferOrderGridListsData();
                }
            };

            $scope.showTransferOrderDetail = function (id) {
                $scope.transferOrderDetailJsonData = [];
                transferOrderWithDetailService.FetchSelectedTransferOrderDetail(
                    id
                ).then(function (value) {
                    $scope.transferOrderDetailJsonData = value.data;
                }, function (reason) {
                    console.log(reason);
                });
            };

            $scope.PrintReport = function (id) {
                var url = applicationBasePath + "/Inventory360Reports/PrintIndTransferOrder?id=" + id + "&user=" + $scope.UserName + "&token=" + userService.GetCurrentUser().access_token;
                var win = window.open(url, '_blank');
                win.focus();
            };

            var GetAllTransferOrderGridListsData = function () {
                var q = $scope.searchTransferOrder === undefined ? '' : $scope.searchTransferOrder;

                allTransferOrderService.FetchAllTransferOrderForReport(
                    q, $scope.pageIndex, dataTableRows
                ).then(function (value) {
                    $scope.allTransferOrderJsonLists = value.data;
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

            GetAllTransferOrderGridListsData();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });