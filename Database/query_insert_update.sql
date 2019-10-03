select * from UserAccounts;

INSERT INTO UserAccounts (UserID, FirstName, LastName, email, UserName, Password, ConfirmPassword)
VALUES(1, 'Alma', 'Lizcano', 'alpilipa2000@yahoo.es', 'DraAlma', 'piel2802','piel2802');

select * from patients;

UPDATE patients
SET created_at = '1988-08-16'
WHERE Cedula = '1098656875';