use students;
select * from student;
select * from name_specialty;
select * from original_documents;
select id_student as '№', lname as 'Фамилия', fname as 'Имя', mname as 'Отчество', average_score as 'Средний бал', original_documents as 'Оригинальные документы', budget as 'Бюджет', name_specialty as 'Наименование специальности', specialty_code as 'Код специальности' from student, name_specialty, original_documents where student.fk_id_name_specialty = name_specialty.id_name_specialty and student.fk_id_original_documents = original_documents.id_original_documents and id_student < 25 order by average_score desc, fk_id_original_documents asc;