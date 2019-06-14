SELECT * FROM patients;
SELECT * FROM treatments;
SELECT * FROM processes;
SELECT * FROM payments;
SELECT * FROM UserAccounts;

SELECT patient_id, name, cellphone_number, gender 
FROM patients
WHERE patient_id BETWEEN 1 AND 10;

--Query Insert in table
INSERT INTO "patients"
VALUES (60,'Aura Rosa Rueda','3212700684',NULL,NULL,'F',1,NULL,'2019-05-16 15:14:46');

INSERT INTO "treatments"
VALUES (20,'Avadment', 500000);

INSERT INTO "processes"
VALUES (133, 60, 1, 0, NULL, 140000, 100000, 0, 600000, 0, '2019-05-16'),
	   (134, 60, 1, 0, NULL, 0, 0, 0, 0, 0, '2019-05-16'),
       (135, 60, 1, 0, NULL, 0, 0, 0, 0, 0, '2019-05-16'),
       (136, 60, 20, 0, NULL, 0, 0, 0, 0, 0, '2019-05-23'),
	   (137, 60, 20, 0, NULL, 0, 0, 0, 0, 0, '2019-05-23'),
       (138, 60, 20, 0, NULL, 0, 0, 0, 0, 0, '2019-05-23');

INSERT INTO "payments"
       VALUES (145,136,0,'cash','should',0,'2019-05-23',0);

DELETE FROM "processes"
       WHERE process_id between 4 and 7;

DELETE FROM "patients"
       WHERE patient_id = 5;
			  
                            
UPDATE treatments
SET name = 'Corona_MP'
WHERE treatment_id = 1; 


--Querys Sarepta Modelos de negocio

--QUERYS Contables
--CUANTO DEBE PAGAR CADA PACIENTE
SELECT pat.patient_id, pat.name, SUM(price)
  FROM processes as pro
  JOIN patients as pat
    ON pro.patient_id = pat.patient_id
  JOIN treatments as t
    ON pro.treatment_id = t.treatment_id
GROUP BY pat.patient_id;

--CUANTO ADEUDA CADA PACIENTE
SELECT pat.patient_id,
       pay.process_id,
       pat.name,
       pay.status,
       price - SUM(pay + discount) AS Cuenta
    FROM  processes as pro
    JOIN  patients as pat
      ON pro.patient_id = pat.patient_id
    JOIN  treatments as t
      ON pro.treatment_id = t.treatment_id
    JOIN  payments as pay
      ON pay.process_id = pro.process_id
GROUP BY pro.process_id;

--Cuales Pacientes han pagado mas
  SELECT pat.patient_id,
         pat.name,
         pay.status,
         SUM(pay) AS PAGO
    FROM  processes as pro
    JOIN  patients as pat
      ON pro.patient_id = pat.patient_id
    JOIN  treatments as t
      ON pro.treatment_id = t.treatment_id
    JOIN  payments as pay
      ON pay.process_id = pro.process_id
GROUP BY pat.patient_id
ORDER BY PAGO DESC
   LIMIT 5;

--Historial Pagos por Pacientes:
SELECT pat.patient_id,
       pat.name,
       t.name,
       pro.tooth,
       SUM(pay) AS PAGO,
       pay.pay_date
  FROM  processes as pro
  JOIN  patients as pat
    ON pro.patient_id = pat.patient_id
  JOIN  treatments as t
    ON pro.treatment_id = t.treatment_id
  JOIN  payments as pay
    ON pay.process_id = pro.process_id
 WHERE pat.name = 'Aura Rosa Rueda'
 GROUP BY pro.process_id;


  --Suma gasto Laboratorio
  SELECT SUM(laboratory) AS Laboratorio
  FROM processes;
  --Suma gasto Consultorio
  SELECT SUM(consultory) AS Consultorio
  FROM processes;
  --Suma gasto Auxiliar
  SELECT SUM(assistant) AS Auxiliar
  FROM processes;
  --Suma gasto Materiales
  SELECT SUM(materials) AS Materiales
  FROM processes;
  --Suma gasto transporte
  SELECT SUM(transport) AS transporte
  FROM processes;
  --Suma gastos Totales
  SELECT SUM(laboratory + consultory + assistant + materials + transport) AS GASTOS
  FROM processes;
  --Factura total Sarepta
  SELECT SUM(pay) AS FACTURADO
  FROM payments;
  --Ganancia total Sarepta
  SELECT SUM(pay) - SUM(laboratory + consultory + assistant + materials + transport) AS GANANCIA
    FROM payments as pay
    JOIN processes as pro
      ON pay.process_id = pro.process_id;


--QUIENES Y CUANTO PAGARON EN X AÑO
SELECT pat.patient_id, pat.name, SUM(pay) AS '2019'
FROM payments as pay
JOIN processes as pro
ON   pay.process_id = pro.process_id
JOIN patients as pat
ON   pat.patient_id = pro.patient_id
WHERE YEAR(`pay_date`) = '2019'
GROUP BY pat.patient_id;
--QUIENES Y CUANTO PAGARON EN X AÑO DESGLOSADO
SELECT pro.process_id, pat.name, pay AS '2019'
FROM payments as pay
JOIN processes as pro
ON   pay.process_id = pro.process_id
JOIN patients as pat
ON   pat.patient_id = pro.patient_id
WHERE YEAR(`pay_date`) = '2019';


--CUANTO PAGARON EN X AÑO
SELECT SUM(pay) AS 'PAGOS 2018'
FROM payments as pay
JOIN processes as pro
ON   pay.process_id = pro.process_id
JOIN patients as pat
ON   pat.patient_id = pro.patient_id
WHERE YEAR(`pay_date`) = '2018';

--CUANTO SE GASTO EN X AÑO
SELECT  SUM(laboratory + consultory + assistant + materials + transport) AS 'GASTOS 2016'
FROM patients as pat
JOIN processes as pro
ON   pat.patient_id = pro.patient_id
WHERE YEAR(`created_at`) = '2016';

--FACTURACION POR UN DETERMINADO PERIODO DEL AÑO
SELECT SUM(pay) AS PRIMER_TRIMESTRE
FROM payments
WHERE YEAR(`pay_date`)= '2016' AND MONTH(`pay_date`)= '01' OR
      YEAR(`pay_date`)= '2016' AND MONTH(`pay_date`)= '02' OR
      YEAR(`pay_date`)= '2016' AND MONTH(`pay_date`)= '03';

SELECT SUM(pay) AS PRIMER_TRIMESTRE
FROM payments
WHERE YEAR(`pay_date`)= '2017' AND MONTH(`pay_date`)= '01' OR
      YEAR(`pay_date`)= '2017' AND MONTH(`pay_date`)= '02' OR
      YEAR(`pay_date`)= '2017' AND MONTH(`pay_date`)= '03';

SELECT SUM(pay) AS PRIMER_TRIMESTRE
FROM payments
WHERE YEAR(`pay_date`)= '2018' AND MONTH(`pay_date`)= '01' OR
      YEAR(`pay_date`)= '2018' AND MONTH(`pay_date`)= '02' OR
      YEAR(`pay_date`)= '2018' AND MONTH(`pay_date`)= '03';

SELECT SUM(pay) AS PRIMER_TRIMESTRE
  FROM payments
  WHERE YEAR(`pay_date`)= '2019' AND MONTH(`pay_date`)= '01' OR
        YEAR(`pay_date`)= '2019' AND MONTH(`pay_date`)= '02' OR
        YEAR(`pay_date`)= '2019' AND MONTH(`pay_date`)= '03';

--REPORTE DE FACTURACION POR MES DE CADA AÑO
SELECT YEAR(`pay_date`) AS ANO,  MONTH(`pay_date`) AS MES, sum(pay)
FROM payments
WHERE YEAR(`pay_date`)= '2017'
GROUP BY MES
ORDER BY MES;

--REPORTE DE FACTURACION POR MES DE TODOS LOS AÑOS
SELECT YEAR(`pay_date`) AS ANO,  MONTH(`pay_date`) AS MES, sum(pay)
FROM payments
WHERE YEAR(`pay_date`)= '2016' AND
      YEAR(`pay_date`)= '2017' AND
      YEAR(`pay_date`)= '2018' AND
      YEAR(`pay_date`)= '2019'
GROUP BY MES;



--Historial de unpaciente especifico
SELECT pat.patient_id,
       pat.name,
       pro.date,
       t.name
  FROM processes as pro
  JOIN patients as pat
    ON pro.patient_id = pat.patient_id
  JOIN treatments as t
    ON pro.treatments_id = t.treatment_id
  JOIN payments as pay
    ON pay.process_id = pro.process_id
 WHERE pat.name ='Antonio Rodriguez';

--Historial de todos los pacientes
SELECT pat.patient_id,
       pat.name,
       pro.date,
       t.name
  FROM processes as pro
  JOIN patients as pat
    ON pro.patient_id = pat.patient_id
  JOIN treatments as t
    ON pro.treatment_id = t.treatment_id
  JOIN payments as pay
    ON pay.process_id = pro.process_id
GROUP BY pro.process_id;
