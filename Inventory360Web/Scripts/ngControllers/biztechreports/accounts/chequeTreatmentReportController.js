myApp
    .controller('chequeTreatmentReportController', ['$scope', '$filter', '$ngConfirm', 'dataTableRows', 'accountService', 'bankCommonService', 'supplierCommonService', 'customerCommonService',
        'applicationBasePath', 'userService', 'customerCommonService', 'bankCommonService', 'chequeTreatmentReportCommonService', 'chequeTreatmentService', 'customerCommonService',
        function (
            $scope, $filter, $ngConfirm, dataTableRows, accountService, bankCommonService, supplierCommonService, customerCommonService, applicationBasePath, userService,
            customerCommonService,
            bankCommonService, chequeTreatmentReportCommonService, chequeTreatmentService, customerCommonService
        ) {
            $scope.selectedCurrency = $scope.DefaultCurrency;
            $scope.dateFrom = $filter('date')(new Date(), "M/d/yyyy");
            $scope.dateTo = $filter('date')(new Date(), "M/d/yyyy");
            $scope.customerOrSupplier = 'Customer';
            $scope.selectedLocationId = 0;

            $scope.changeOption = function () {                
                if ($scope.chequeTypeValue === "issuedCheque" || $scope.reportNameValue == 'AdvanceChequeIssued') {
                    $scope.dateFrom = "";
                    $scope.dateTo = "";
                    $scope.customerOrSupplier = "Supplier";
                    $scope.collectionOrPayment = "Payment";
                    $scope.CustomerOrSupplier = "Supplier";
                    $scope.allBankJsonData = $scope.tempOwnBankJsonData;
                }
                else {
                    $scope.customerOrSupplier = "Customer";
                    $scope.collectionOrPayment = "Collection";
                    $scope.CustomerOrSupplier = "Customer";
                    $scope.allBankJsonData = $scope.tempAllBankJsonData;
                }  
                GetChequeCollectionOrPaymentDateOptionByGroupForDropdown();
            };

            // load all company
            var getAllCompanyForDropdown = function () {
                accountService.fetchAllCompany().then(function (value) {
                    $scope.allCompanyJsonData = value.data;
                    if (value.data.length === 1) {
                        $scope.companyId = value.data[0].CompanyId.toString();
                        $scope.getLocationByCompanyForDropdown();
                    }
                }, function (reason) {
                    console.log(reason);
                });
            }
            // load all parent location by selected company
            $scope.getLocationByCompanyForDropdown = function () {
                accountService.fetchLocationByCompany($scope.companyId).then(function (value) {
                    $scope.allLocationByCompanyJsonData = value.data;
                    if (value.data.length === 2) {
                        $scope.selectedLocationId = value.data[1].Value.toString();
                    }
                    else {
                        $.each(value.data, function () {
                            if (this.IsSelected) {
                                $scope.selectedLocationId = this.Value;
                            }
                        });
                    }
                }, function (reason) {
                    console.log(reason);
                });
            };
            // load ChequeType or Cheque Performance Type
            var GetChequeTypeForDropdown = function () {
                if ($scope.reportNameValue == "CustomerSupplierwiseChequePerformance") {
                    chequeTreatmentService.GetChequePerformanceType().then(function (value) {
                        $scope.chequeTypeDropDownJsonData = value.data;
                        $scope.chequeTypeValue = value.data[0].Value;
                    }, function (reason) {
                        console.log(reason);
                    });
                }
                else {
                    chequeTreatmentService.GetChequeType().then(function (value) {
                        $scope.chequeTypeDropDownJsonData = value.data;
                        $scope.chequeTypeValue = value.data[0].Value;
                    }, function (reason) {
                        console.log(reason);
                    });
                }
            }
            //load all bank           
            var getAllBankForDropdown = function () {
                bankCommonService.FetchBank("").then(function (value) {
                    $scope.allBankJsonData = value.data;
                    $scope.tempAllBankJsonData = angular.copy($scope.allBankJsonData);
                    $scope.bankId = "0";
                }, function (reason) {
                    console.log(reason);
                });
            }

            //load own bank           
            var getOwnBankByCompanyIdForDropdown = function () {
                bankCommonService.FetchOwnBank("").then(function (value) {
                    $scope.tempOwnBankJsonData = value.data;
                }, function (reason) {
                    console.log(reason);
                });
            }
            // load ReportName
            var GetReportNameForDropdown = function () {
                chequeTreatmentReportCommonService.GetReportName().then(function (value) {
                    $scope.reportNameDropDownJsonData = value.data;
                    $scope.reportNameValue = value.data[0].Value;
                    console.log($scope.reportNameValue);
                }, function (reason) {
                    console.log(reason);
                });
            }
            // load CustomerGroupAll
            var GetCustomerGroupAllForDropdown = function () {
                customerCommonService.GetCustomerGroupAll().then(function (value) {
                    $scope.CustomerGroupAllDropDownJsonData = value.data;
                    $scope.CustomerGroupId = "0";
                }, function (reason) {
                    console.log(reason);
                });
            }
            // load PositionOption
            var GetPositionOptionNameForDropdown = function () {
                chequeTreatmentReportCommonService.GetPositionOptionName().then(function (value) {
                    $scope.positionOptionNameDropDownJsonData = value.data;
                    $scope.dataPositionOptionValue = value.data[0].Value;
                }, function (reason) {
                    console.log(reason);
                });
            }
            // load ChequeStatus
            var GetChequeStatusForDropdown = function () {
                chequeTreatmentService.GetChequeStatus().then(function (value) {
                    $scope.chequeStatusDropDownJsonData = value.data;
                }, function (reason) {
                    console.log(reason);
                });
            }
            // load ChequeOrTreatementBankOption
            var GetChequeOrTreatementBankOptionForDropdown = function () {
                chequeTreatmentReportCommonService.GetChequeOrTreatementBankOption().then(function (value) {
                    $scope.chequeOrTreatementBankOptionDropDownJsonData = value.data;
                    $scope.chequeOrTreatementBankOptionValue = value.data[0].Value;
                }, function (reason) {
                    console.log(reason);
                });
            }
            // load GetChequeCollectionOrPaymentDateOptionByGroup
            var GetChequeCollectionOrPaymentDateOptionByGroupForDropdown = function () {
                chequeTreatmentReportCommonService.GetChequeCollectionOrPaymentDateOptionByGroup(($scope.chequeTypeValue == undefined ? "receivedCheque" : $scope.chequeTypeValue)).then(function (value) {
                    $scope.chequeCollectionOrPaymentDateOptionByGroupDropDownJsonData = value.data;
                    $scope.chequeCollectionOrPaymentDateOptionValue = value.data[0].Value;
                }, function (reason) {
                    console.log(reason);
                });
            }
            //getCustomerOrSupplierName
            $scope.getCustomerOrSupplierName = function () {
                var q = $scope.customerOrSupplierName === undefined ? '' : $scope.customerOrSupplierName;
                var customerGroupId = $scope.CustomerGroupId === undefined ? 0 : $scope.CustomerGroupId;

                if (q.length >= 3) {
                    if ($scope.chequeTypeValue === "issuedCheque" || $scope.reportNameValue == 'AdvanceChequeIssued') {
                        supplierCommonService.FetchAllSupplier(q).then(function (value) {
                            $scope.customerOrSupplierNameJsonData = value.data;
                        });
                    }
                    else {
                        customerCommonService.FetchAllCustomerByGroup(q,customerGroupId).then(function (value) {
                            $scope.customerOrSupplierNameJsonData = value.data;
                        });
                    }
                } else {
                    $scope.selectedCustomerOrSupplierId = 0;
                    $scope.customerOrSupplierNameJsonData = null;
                }
            };
            $scope.fillCustomerTextbox = function (obj) {
                $scope.customerOrSupplierName = '';
                $scope.selectedCustomerOrSupplierId = Number(obj.Value);
                $scope.customerOrSupplierName = obj.Item;
                $scope.customerOrSupplierNameJsonData = null;
                //GetAllChequeGridListsData();
            };
            //getChequeNo
            $scope.getChequeNo = function () {
                var query = $scope.chequeNo === undefined ? '' : $scope.chequeNo;

                if (query.length >= 3) {
                    chequeTreatmentReportCommonService.GetChequeNo(query).then(function (value) {
                        $scope.chequeNoJsonData = value.data;
                    });
                } else {
                    $scope.selectedchequeNo = "";
                    $scope.chequeNoJsonData = null;
                }
            };
            $scope.fillChequeNoTextbox = function (obj) {                
                $scope.selectedchequeNo = Number(obj.Value);                
                $scope.chequeNoJsonData = null;                
            };
            //changeReportName
            $scope.changeReportName = function () {
                $scope.changeOption();   
                GetChequeCollectionOrPaymentDateOptionByGroupForDropdown();
                GetChequeTypeForDropdown();
            }
            //open report
            $scope.PrintReport = function () {
                var url = applicationBasePath + "/Inventory360Reports/PrintChequeTreatement?ReportName=" + $scope.reportNameValue + "&chequeType=" + ($scope.chequeTypeValue === undefined ? "" : $scope.chequeTypeValue) + "&dataPositionOptionValue=" + $scope.dataPositionOptionValue + "&LocationId=" + ($scope.selectedLocationId === undefined ? 0 : $scope.selectedLocationId) + "&bankId=" + ($scope.bankId === undefined ? 0 : $scope.bankId) + "&customerOrSupplierId=" + ($scope.selectedCustomerOrSupplierId === undefined ? 0 : $scope.selectedCustomerOrSupplierId) + "&dateFrom=" + $scope.dateFrom + "&dateTo=" + $scope.dateTo + "&chequeStatusCode=" + ($scope.chequeStatusCode === undefined ? "" : $scope.chequeStatusCode) + "&chequeOrTreatementBankOptionValue=" + $scope.chequeOrTreatementBankOptionValue + "&chequeCollectionOrPaymentDateOptionValue=" + $scope.chequeCollectionOrPaymentDateOptionValue + "&user=" + $scope.UserName + "&currency=" + $scope.currencyId + "&token=" + userService.GetCurrentUser().access_token;
                var win = window.open(url, '_blank');
                win.focus();
            };

            getAllCompanyForDropdown();
            GetReportNameForDropdown();
            GetPositionOptionNameForDropdown();
            getAllBankForDropdown();
            getOwnBankByCompanyIdForDropdown();
            GetChequeStatusForDropdown();
            GetChequeTypeForDropdown();
            GetChequeOrTreatementBankOptionForDropdown();
            GetChequeCollectionOrPaymentDateOptionByGroupForDropdown();
            GetCustomerGroupAllForDropdown();

        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });