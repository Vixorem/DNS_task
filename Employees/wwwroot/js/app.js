$(function () {
    EmpRequest.GetEmployees(EmpRequest.SetUpEmployees);
    $("#add").click(function (event) {
        event.preventDefault();
        console.log("#add clicked");
        EmpRequest.GetCreate();
    });
});
var Position = /** @class */ (function () {
    function Position() {
    }
    return Position;
}());
var Department = /** @class */ (function () {
    function Department() {
    }
    return Department;
}());
var Employee = /** @class */ (function () {
    function Employee() {
    }
    return Employee;
}());
var EmpRequest = /** @class */ (function () {
    function EmpRequest() {
    }
    EmpRequest.SetUpEmployees = function (data) {
        var table = document.createElement("table");
        table.className = "tablestyle";
        var thead = document.createElement("thead");
        thead.className = "theadstyle";
        var tr = document.createElement("tr");
        tr.className = "trstyle";
        for (var i = 0; i < EmpRequest.tableHeaders.length; ++i) {
            var th = document.createElement("th");
            th.className = "thstyle";
            th.textContent = EmpRequest.tableHeaders[i];
            tr.appendChild(th);
        }
        thead.appendChild(tr);
        table.appendChild(thead);
        var tbody = document.createElement("tbody");
        for (var i = 0; i < data.length; ++i) {
            tr = document.createElement("tr");
            tr.id = "tr" + String(data[i].id);
            var td1 = document.createElement("td");
            td1.className = "tdstyle";
            td1.textContent = data[i].name;
            var td2 = document.createElement("td");
            td2.className = "tdstyle";
            td2.textContent = data[i].secondname;
            var td3 = document.createElement("td");
            td3.className = "tdstyle";
            td3.textContent = data[i].surname;
            var td4 = document.createElement("td");
            td4.className = "tdstyle";
            td4.textContent = data[i].position.name;
            var td5 = document.createElement("td");
            td5.className = "tdstyle";
            td5.textContent = data[i].department.name;
            if (data[i].boss.surname == null) {
                data[i].boss.surname = "";
            }
            var td6 = document.createElement("td");
            td6.className = "tdstyle";
            td6.textContent = data[i].boss.surname;
            var td7 = document.createElement("td");
            td7.className = "tdstyle";
            td7.textContent = data[i].recruitDate;
            var td8 = document.createElement("td");
            td8.className = "tdstyle";
            var a1 = document.createElement("a");
            a1.className = "editButton";
            a1.href = "/Employees/Edit/" + data[i].id;
            a1.textContent = "Изменить";
            a1.addEventListener("click", function (e) {
                e.preventDefault();
                EmpRequest.GetEdit(EmpRequest.SetUpEdit, a1.href);
            }, false);
            var a2 = document.createElement("a");
            a2.className = "bossesButton";
            a2.href = "/Employees/Bosses/" + data[i].id;
            a2.textContent = "Руководители";
            a2.addEventListener("click", function (e) {
                e.preventDefault();
                EmpRequest.GetBosses(EmpRequest.SetUpEdit, a2.href);
            }, false);
            var a3 = document.createElement("a");
            a3.className = "deleteButton";
            a1.id = "dl" + String(data[i].id);
            a3.href = "/Employees / Delete /" + data[i].id;
            a3.textContent = "Удалить";
            a3.addEventListener("click", function (e) {
                e.preventDefault();
                EmpRequest.SetUpDelete(a3.href);
            }, false);
            var tr21 = document.createElement("tr");
            var tr22 = document.createElement("tr");
            var tr23 = document.createElement("tr");
            var td21 = document.createElement("td");
            var td22 = document.createElement("td");
            var td23 = document.createElement("td");
            td21.appendChild(a1);
            td22.appendChild(a2);
            td23.appendChild(a3);
            tr21.appendChild(td21);
            tr22.appendChild(td22);
            tr23.appendChild(td23);
            td8.appendChild(tr21);
            td8.appendChild(tr22);
            td8.appendChild(tr23);
            tr.appendChild(td1);
            tr.appendChild(td2);
            tr.appendChild(td3);
            tr.appendChild(td4);
            tr.appendChild(td5);
            tr.appendChild(td6);
            tr.appendChild(td7);
            tr.appendChild(td8);
            tbody.appendChild(tr);
        }
        table.appendChild(tbody);
        $("#empTable").html(table);
    };
    EmpRequest.GetEmployees = function (f) {
        console.log("GET request for LoadEmployees()");
        $.getJSON("/Employees/GetEmployees", function (data) {
        }).done(function (data) {
            console.log("GET succeed");
            f(data);
        }).fail(function (data) {
            console.log("GET failed");
        });
    };
    EmpRequest.SetUpDelete = function (ref) {
    };
    EmpRequest.SetUpEdit = function () {
    };
    EmpRequest.GetEdit = function (f, ref) {
        console.log("GET request for LoadEdit()");
        $.getJSON(ref, function (data) {
        }).done(function (data) {
            console.log("GET succeed");
            f();
        }).fail(function (data) {
            console.log("GET failed");
        });
    };
    EmpRequest.GetBosses = function (f, ref) {
        console.log("GET request for LoadBosses()");
        $.getJSON(ref, function (data) {
        }).done(function (data) {
            console.log("GET succeed");
            f();
        }).fail(function (data) {
            console.log("GET failed");
        });
    };
    EmpRequest.SetUpBosses = function () {
    };
    EmpRequest.GetCreate = function () {
        console.log("GET request for GetSelections()");
        $.getJSON("Employees/GetSelections/", function (data) {
        }).done(function (data) {
            console.log("GET succeed");
            $("#appearingLayout").load("Employees/Create", function () {
                $("#cancel").click(function (event) {
                    event.preventDefault();
                    console.log("#cancel clicked");
                    EmpRequest.RemoveAppearingHtml();
                });
                $("#create").click(function (event) {
                    event.preventDefault();
                    console.log("#create clicked");
                    EmpRequest.PostCreate();
                });
                EmpRequest.SetUpCreate(data);
            });
        }).fail(function (data) {
            console.log("GET failed");
        });
    };
    EmpRequest.SetUpCreate = function (data) {
        var bosses = data["bosses"];
        var positions = data["positions"];
        var departments = data["departments"];
        for (var i = 0; i < bosses.length; ++i) {
            var opt = document.createElement("option");
            opt.value = String(bosses[i].id);
            opt.textContent = bosses[i].surname;
            $("#bossSelection").append(opt);
        }
        for (var i = 0; i < positions.length; ++i) {
            var opt = document.createElement("option");
            opt.value = String(positions[i].id);
            opt.textContent = positions[i].name;
            $("#positionSelection").append(opt);
        }
        for (var i = 0; i < departments.length; ++i) {
            var opt = document.createElement("option");
            opt.value = String(departments[i].id);
            opt.textContent = departments[i].name;
            $("#departmentSelection").append(opt);
        }
        var today = new Date();
        var date = document.getElementById("recdate");
        var y = today.getFullYear();
        var m = (String(today.getMonth()).length == 1) ? ("0" + today.getMonth()) : (today.getMonth());
        var d = (String(today.getDate()).length == 1) ? ("0" + today.getDate()) : (today.getDate());
        date.value = y + "-" + m + "-" + d;
    };
    EmpRequest.PostCreate = function () {
        console.log("POST to Create");
        var emp = new Employee();
        var bossSelect = document.getElementById("bossSelection");
        var posSelect = document.getElementById("positionSelection");
        var depSelect = document.getElementById("departmentSelection");
        emp.Id = null;
        emp.Name = document.getElementById("name").value;
        emp.Secondname = document.getElementById("secondname").value;
        emp.Surname = document.getElementById("surname").value;
        emp.BossId = Number(bossSelect[bossSelect.selectedIndex].value);
        emp.PositionId = Number(posSelect[posSelect.selectedIndex].value);
        emp.DepartmentId = Number(depSelect[depSelect.selectedIndex].value);
        emp.Boss = null;
        emp.Position = null;
        emp.Department = null;
        var date = new Date(document.getElementById("recdate").value);
        emp.RecruitDate = date.toISOString();
        $.ajax({
            type: "POST",
            url: "Employees/Create/",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(emp),
            success: function (response) {
                if (response.success) {
                    console.log("POST succeed");
                    EmpRequest.RemoveAppearingHtml();
                }
                else {
                    alert("Сервер не смог обработать запрос, " +
                        "возможно, перезагрузка страницы cможет помочь");
                    console.log("Controller returned error");
                }
            },
            error: function (response) {
                console.log("POST failed");
            }
        });
    };
    EmpRequest.RemoveAppearingHtml = function () {
        document.getElementById("appearingLayout").innerHTML = "";
    };
    EmpRequest.tableHeaders = [
        "Имя",
        "Отчество",
        "Фамилия",
        "Должность",
        "Отдел",
        "Руководитель",
        "Дата трудоустройства",
        "Действия",
    ];
    return EmpRequest;
}());
//# sourceMappingURL=app.js.map