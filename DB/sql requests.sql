use students;
select * from students;
select * from groups;
select * from original_documents;
select id_students as '№', lname as 'Фамилия', fname as 'Имя', mname as 'Отчество', average_score as 'Средний бал', original_documents as 'Оригинальные документы', budget as 'Бюджет', groups.group as 'Группа' from students, groups, original_documents where students.fk_id_groups = groups.id_groups and students.fk_id_original_documents = original_documents.id_original_documents and id_students < 25 order by average_score desc, fk_id_original_documents asc;