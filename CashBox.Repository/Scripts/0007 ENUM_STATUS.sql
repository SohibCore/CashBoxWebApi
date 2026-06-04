DELETE FROM "ENUM_STATUS";
ALTER SEQUENCE "ENUM_STATUS_ID_seq" RESTART WITH 1;

INSERT INTO "ENUM_STATUS" ("CODE", "SHORT_NAME", "FULL_NAME") 
VALUES 
('CREATED',     'Yaratilgan',      'Yaratilgan holat'),
('ACCEPT',      'Qabul qilingan',  'Qabul qilingan holat'),
('NOT_ACCEPT',  'Rad etilgan',     'Rad etilgan holat'),
('MODIFIED',    'O''zgartirilgan', 'O''zgartirilgan holat'),
('DELETE',      'O''chirilgan',    'O''chirilgan holat');
