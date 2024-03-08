myApp
    .controller('problemSetupController', ['$scope', '$ngConfirm', 'dataTableRows',
        'problemSetupService', 'operationalEventCommonService', 'operationalSubEventCommonService',
        function (
            $scope, $ngConfirm, dataTableRows,
            problemSetupService, operationalEventCommonService, operationalSubEventCommonService
        ) {
            $scope.saveMode = true;
            $scope.problemId = 0;
            $scope.pageIndex = 1;
            $scope.showRecordsInfo = '';
            $scope.showGrid = false;
            $scope.allProblemJsonLists = [];

            $scope.previous = function (e) {
                e.preventDefault();
                if ($scope.pageIndex > 1) {
                    $scope.pageIndex--;
                    GetAllProblemSetupGridListsData();
                }
            };

            $scope.next = function (e) {
                e.preventDefault();
                if ($scope.pageIndex < lastPageNo) {
                    $scope.pageIndex++;
                    GetAllProblemSetupGridListsData();
                }
            };

            $scope.filterProblem = function () {
                if ($scope.searchProblem !== undefined) {
                    $scope.pageIndex = 1;
                    GetAllProblemSetupGridListsData();
                }
            };

            //load sub event
            $scope.getSelectedEventSubEventForDropdown = function () {
                operationalSubEventCommonService.FetchOperationalSubEvent(
                    "", $scope.selectedEventName
                ).then(function (value) {
                    $scope.operationalSubEventJsonData = value.data;
                }, function (reason) {
                    console.log(reason);
                });
            };

            $scope.clickSave = function () {
                problemSetupService.SaveProblemSetup(
                    $scope.selectedEventName, $scope.selectedSubEventName, $scope.problemName
                ).then(function (value) {
                    // alert
                    $ngConfirm({
                        title: value.data.IsSuccess === true ? 'Success' : 'Failed',
                        icon: 'glyphicon glyphicon-info-sign',
                        theme: 'supervan',
                        content: value.data.Message,
                        animation: 'scale',
                        buttons: {
                            Ok: {
                                btnClass: "btn-blue"
                            }
                        },
                    });

                    if (value.data.IsSuccess) {
                        $scope.saveMode = true;
                        $scope.problemName = undefined;
                        $scope.selectedEventName = undefined;
                        $scope.selectedSubEventName = undefined;
                        GetAllProblemSetupGridListsData();
                    }
                });
            };

            $scope.clickToEdit = function (item) {
                $scope.saveMode = false;
                $scope.problemId = item.ProblemId;
                $scope.problemName = item.Name;
                $scope.selectedEventName = item.EventName;
                $scope.getSelectedEventSubEventForDropdown();
                $scope.selectedSubEventName = item.SubEventName;
            };

            $scope.clickUpdate = function () {
                problemSetupService.UpdateProblemSetup(
                    $scope.problemId, $scope.selectedEventName, $scope.selectedSubEventName, $scope.problemName
                ).then(function (value) {
                    // alert
                    $ngConfirm({
                        title: value.data.IsSuccess === true ? 'Success' : 'Failed',
                        icon: 'glyphicon glyphicon-info-sign',
                        theme: 'supervan',
                        content: value.data.Message,
                        animation: 'scale',
                        buttons: {
                            Ok: {
                                btnClass: "btn-blue"
                            }
                        },
                    });

                    if (value.data.IsSuccess) {
                        $scope.problemId = 0;
                        $scope.saveMode = true;
                        $scope.problemName = undefined;
                        $scope.selectedEventName = undefined;
                        $scope.selectedSubEventName = undefined;
                        GetAllProblemSetupGridListsData();
                    }
                });
            };

            //load all Event
            var getAllEventForDropdown = function () {
                operationalEventCommonService.FetchOperationalEvent("").then(function (value) {
                    $scope.operationalEventJsonData = value.data;
                    //if (value.data.length > 0) {
                    //    $scope.selectedEventName = value.data[0].Value.toString();
                    //    $scope.getSelectedEventSubEventForDropdown($scope.selectedEventName);
                    //}
                }, function (reason) {
                    console.log(reason);
                });
            }

            // load all problem lists
            var GetAllProblemSetupGridListsData = function () {
                var q = $scope.searchProblem === undefined ? '' : $scope.searchProblem;

                problemSetupService.FetchProblemLists(
                    q, $scope.pageIndex, dataTableRows
                ).then(function (value) {
                    $scope.allProblemJsonLists = value.data;
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
            };

            getAllEventForDropdown();
            GetAllProblemSetupGridListsData();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });