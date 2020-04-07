$(function () {
    EmpRequest.GetEmployees(DocManager.SetUpEmployees);
    $("#add").click(function (event) {
        event.preventDefault();
        console.log("#add clicked");
        EmpRequest.GetCreate(DocManager.SetUpSelections);
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
    EmpRequest.PostEdit = function (id) {
        console.log("POST to EditConfirmed");
        var emp = new Employee();
        var bossSelect = document.getElementById("bossSelection");
        var posSelect = document.getElementById("positionSelection");
        var depSelect = document.getElementById("departmentSelection");
        emp.Id = id;
        emp.Name = document.getElementById("name").value;
        emp.Secondname = document.getElementById("secondname").value;
        emp.Surname = document.getElementById("surname").value;
        emp.BossId = Number(bossSelect[bossSelect.selectedIndex].value);
        emp.PositionId = Number(posSelect[posSelect.selectedIndex].value);
        emp.DepartmentId = Number(depSelect[depSelect.selectedIndex].value);
        emp.Boss = new Employee();
        emp.Boss.Id = emp.BossId;
        emp.Boss.Surname = bossSelect[bossSelect.selectedIndex].textContent;
        emp.Position = new Position();
        emp.Position.Id = emp.PositionId;
        emp.Position.Name = posSelect[posSelect.selectedIndex].textContent;
        emp.Department = new Department();
        emp.Department.Id = emp.DepartmentId;
        emp.Department.Name = depSelect[depSelect.selectedIndex].textContent;
        var date = new Date(document.getElementById("recdate").value);
        emp.RecruitDate = date.toISOString();
        $.ajax({
            type: "POST",
            url: "Employees/EditComfirmed/",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(emp),
            success: function (response) {
                if (response.success) {
                    console.log("POST succeed");
                    DocManager.RemoveAppearingHtml();
                    DocManager.UpdateRow(id, response.emp);
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
    EmpRequest.GetEdit = function (f, id) {
        console.log("GET request for Edit()");
        console.log("GET request for GetSelections()");
        $("#appearingLayout").load("Employees/Edit/", function () {
            $.getJSON("Employees/GetSelections/", function (data) {
                DocManager.SetUpSelections(data);
            }).done(function () {
                console.log("GET succeed");
                $.getJSON("Employees/GetEmployee/" + id).done(function (data) {
                    console.log("GET succeed");
                    $("#cancel").click(function (event) {
                        event.preventDefault();
                        console.log("#cancel clicked");
                        DocManager.RemoveAppearingHtml();
                    });
                    $("#delete").click(function (event) {
                        event.preventDefault();
                        console.log("#edit clicked");
                        EmpRequest.PostEdit(id);
                    });
                    f(data.employee, id);
                }).fail(function (data) {
                    console.log("GET failed");
                });
            }).fail(function () {
                console.log("GET failed");
            });
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
    EmpRequest.GetCreate = function (f) {
        console.log("GET request for GetSelections()");
        $.getJSON("Employees/GetSelections/", function (data) {
        }).done(function (data) {
            console.log("GET succeed");
            $("#appearingLayout").load("Employees/Create", function () {
                $("#cancel").click(function (event) {
                    event.preventDefault();
                    console.log("#cancel clicked");
                    DocManager.RemoveAppearingHtml();
                });
                $("#create").click(function (event) {
                    event.preventDefault();
                    console.log("#create clicked");
                    EmpRequest.PostCreate();
                });
                f(data);
            });
        }).fail(function (data) {
            console.log("GET failed");
        });
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
                    DocManager.RemoveAppearingHtml();
                    //TODO: если по пагинации норм, то вставить
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
    EmpRequest.GetDelete = function (f, id) {
        console.log("GET request for Delete()");
        $("#appearingLayout").load("Employees/Delete/", function () {
            console.log("GET succeed");
            $("#cancel").click(function (event) {
                event.preventDefault();
                console.log("#cancel clicked");
                DocManager.RemoveAppearingHtml();
            });
            $("#delete").click(function (event) {
                event.preventDefault();
                console.log("#delete clicked");
                EmpRequest.PostDelete(id);
            });
            f(id);
        });
    };
    EmpRequest.PostDelete = function (id) {
        console.log("POST to Delete()");
        $.ajax({
            type: "POST",
            url: "Employees/DeleteConfirmed/" + id,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(id),
            success: function (response) {
                if (response.success) {
                    console.log("POST succeed");
                    DocManager.RemoveAppearingHtml();
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
var DocManager = /** @class */ (function () {
    function DocManager() {
    }
    DocManager.RemoveAppearingHtml = function () {
        document.getElementById("appearingLayout").innerHTML = "";
    };
    DocManager.SetUpSelections = function (data) {
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
        var date = document.getElementById("recdate");
        date.value = DocManager.ToHTMLDate(new Date());
    };
    DocManager.SetUpBosses = function () {
    };
    DocManager.SetUpDelete = function (id) {
        var children = $("#" + id).children();
        var name = children[0].textContent;
        var secondname = children[1].textContent;
        var surname = children[2].textContent;
        var pos = children[3].textContent;
        var dep = children[4].textContent;
        $("#info").text(name
            + " " + secondname + " " + surname +
            " " + "(" + dep + ", " + pos + ")");
    };
    DocManager.SetUpEdit = function (emp, id) {
        $("#name").val(emp.name);
        $("#secondname").val(emp.secondname);
        $("#surname").val(emp.surname);
        $("#bossSelection").val(emp.bossId);
    };
    DocManager.SetUpEmployees = function (data) {
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
        var _loop_1 = function () {
            tr = document.createElement("tr");
            tr.id = String(data[i].id);
            td1 = document.createElement("td");
            td1.className = "tdstyle";
            td1.textContent = data[i].name;
            td2 = document.createElement("td");
            td2.className = "tdstyle";
            td2.textContent = data[i].secondname;
            td3 = document.createElement("td");
            td3.className = "tdstyle";
            td3.textContent = data[i].surname;
            td4 = document.createElement("td");
            td4.className = "tdstyle";
            td4.textContent = data[i].position.name;
            td5 = document.createElement("td");
            td5.className = "tdstyle";
            td5.textContent = data[i].department.name;
            if (data[i].boss.surname == null) {
                data[i].boss.surname = "";
            }
            td6 = document.createElement("td");
            td6.className = "tdstyle";
            td6.textContent = data[i].boss.surname;
            td7 = document.createElement("td");
            td7.className = "tdstyle";
            td7.textContent = DocManager.ToReadableDate(new Date(data[i].recruitDate));
            td8 = document.createElement("td");
            td8.className = "tdstyle";
            a1 = document.createElement("a");
            a1.className = "editButton";
            //a1.href = "/Employees/Edit/" + data[i].id;
            a1.textContent = "Изменить";
            var id = data[i].id;
            a1.addEventListener("click", function (e) {
                e.preventDefault();
                EmpRequest.GetEdit(DocManager.SetUpEdit, id);
            }, false);
            a2 = document.createElement("a");
            a2.className = "bossesButton";
            a2.href = "/Employees/Bosses/" + data[i].id;
            a2.textContent = "Руководители";
            a2.addEventListener("click", function (e) {
                e.preventDefault();
                EmpRequest.GetBosses(DocManager.SetUpEdit, a2.href);
            }, false);
            a3 = document.createElement("a");
            a3.className = "deleteButton";
            //a3.href = "/Employees/Delete/" + data[i].id;
            a3.textContent = "Удалить";
            a3.addEventListener("click", function (e) {
                e.preventDefault();
                EmpRequest.GetDelete(DocManager.SetUpDelete, id);
            }, false);
            tr21 = document.createElement("tr");
            tr22 = document.createElement("tr");
            tr23 = document.createElement("tr");
            td21 = document.createElement("td");
            td22 = document.createElement("td");
            td23 = document.createElement("td");
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
        };
        var td1, td2, td3, td4, td5, td6, td7, td8, a1, a2, a3, tr21, tr22, tr23, td21, td22, td23;
        for (var i = 0; i < data.length; ++i) {
            _loop_1();
        }
        table.appendChild(tbody);
        $("#empTable").html(table);
    };
    DocManager.UpdateRow = function (id, emp) {
        var children = $("#" + id).children();
        children[0].textContent = emp.Name;
        children[1].textContent = emp.Secondname;
        children[2].textContent = emp.Surname;
        children[3].textContent = emp.Position.Name;
        children[4].textContent = emp.Department.Name;
        children[5].textContent = emp.Boss.Surname;
        children[4].textContent = DocManager.ToReadableDate(emp.RecruitDate);
    };
    DocManager.ToReadableDate = function (date) {
        var y = date.getFullYear();
        var m = (String(date.getMonth()).length == 1) ?
            ("0" + date.getMonth()) : (date.getMonth());
        var d = (String(date.getDate()).length == 1) ?
            ("0" + date.getDate()) : (date.getDate());
        return d + "." + m + "." + y;
    };
    DocManager.ToHTMLDate = function (date) {
        var y = date.getFullYear();
        var m = (String(date.getMonth()).length == 1) ?
            ("0" + date.getMonth()) : (date.getMonth());
        var d = (String(date.getDate()).length == 1) ?
            ("0" + date.getDate()) : (date.getDate());
        return y + "-" + m + "-" + d;
    };
    return DocManager;
}());
//# sourceMappingURL=app.js.map