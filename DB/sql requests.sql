use students;
select * from students;
select * from groups;
select * from original_documents;
select * from budget;
select id_student as '№', lname as 'Фамилия', fname as 'Имя', mname as 'Отчество', average_score as 'Средний бал', original_documents as 'Оригинальные документы', budget as 'Бюджет', groups.group as 'Группа' from students, groups, original_documents, budget where students.fk_id_groups = groups.id_groups and students.fk_id_original_documents = original_documents.id_original_documents and students.fk_id_budget = budget.id_budget order by average_score desc, fk_id_original_documents asc;