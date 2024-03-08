myApp
    .controller('transferRequisitionReportController', ['$scope', 'dataTableRows', 'applicationBasePath', 'userService',
        'transferRequisitionFinalizeWithDetailService', 'allTransferRequisitionFinalizeService',
        function (
            $scope, dataTableRows, applicationBasePath, userService,
            transferRequisitionFinalizeWithDetailService, allTransferRequisitionFinalizeService
        ) {
            $scope.pageIndex = 1;
            $scope.showRecordsInfo = '';
            $scope.showGrid = false;
            $scope.alltransferRequisitionJsonLists = [];

            $scope.previous = function (e) {
                e.preventDefault();
                if ($scope.pageIndex > 1) {
                    $scope.pageIndex--;
                    GetAllTransferRequisitionGridListsData();
                }
            };

            $scope.next = function (e) {
                e.preventDefault();
                if ($scope.pageIndex < lastPageNo) {
                    $scope.pageIndex++;
                    GetAllTransferRequisitionGridListsData();
                }
            };

            $scope.filterTransferRequisitionNo = function () {
                if ($scope.searchTransferRequisitionNo !== undefined) {
                    $scope.pageIndex = 1;
                    GetAllTransferRequisitionGridListsData();
                }
            };

            $scope.showRequisitionFinalizeDetail = function (id) {
                $scope.requisitionFinalizeDetailJsonData = [];
                transferRequisitionFinalizeWithDetailService.FetchSelectedFinalizeDetail(
                    id
                ).then(function (value) {
                    $scope.requisitionFinalizeDetailJsonData = value.data;
                }, function (reason) {
                    console.log(reason);
                });
            };

            $scope.PrintReport = function (id) {
                var url = applicationBasePath + "/Inventory360Reports/PrintIndTransferRequisition?id=" + id + "&user=" + $scope.UserName + "&token=" + userService.GetCurrentUser().access_token;
                var win = window.open(url, '_blank');
                win.focus();
            };

            var GetAllTransferRequisitionGridListsData = function () {
                var q = $scope.searchTransferRequisitionNo === undefined ? '' : $scope.searchTransferRequisitionNo;

                allTransferRequisitionFinalizeService.FetchAllTransferRequisitionFinalize(
                    q, $scope.pageIndex, dataTableRows
                ).then(function (value) {
                    $scope.alltransferRequisitionJsonLists = value.data;
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

            GetAllTransferRequisitionGridListsData();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });