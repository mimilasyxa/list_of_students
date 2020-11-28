use students;
select * from student;
select * from name_specialty;
select * from original_documents;
select id_student as '№', lname as 'Фамилия', fname as 'Имя', mname as 'Отчество', average_score as 'Средний бал', original_documents as 'Оригинальные документы', name_specialty as 'Наименование специальности', specialty_code as 'Код специальности' from student, name_specialty, original_documents where student.fk_id_name_specialty = name_specialty.id_name_specialty and student.fk_id_original_documents = original_documents.id_original_documents order by average_score desc, fk_id_original_documents asc limit 100; #после limit подставишь переменную из max_countPlebs это будет первой сортировкой и точно тоже самое сделаешь и во второй раз только с переменной max_countFreePlebs