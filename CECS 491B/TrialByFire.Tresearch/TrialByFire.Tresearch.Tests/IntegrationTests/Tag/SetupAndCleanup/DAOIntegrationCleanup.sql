DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO test tag1';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO test tag1';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO This Tag Exists Already';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO This Tag Exists Already';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Delete Me Tag';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Delete Me Tag';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Add Tag1';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Add Tag1';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Add Tag2';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Add Tag2';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Add Tag3';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Add Tag3';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Add Tag4';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Add Tag4';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Add Tag5';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Add Tag5';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Delete Tag1';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Delete Tag1';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Delete Tag2';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Delete Tag2';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Delete Tag3';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Delete Tag3';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Delete Tag4';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Delete Tag4';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Get Tag1';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Get Tag1';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Get Tag2';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Get Tag2';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Get Tag3';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Get Tag3';
DELETE NodeTags WHERE TagName = 'Tresearch SqlDAO Get Tag4';
DELETE Tags WHERE TagName = 'Tresearch SqlDAO Get Tag4';

DELETE Nodes WHERE UserHash = '27590a1ba0a96ce95c8681e81181ecca27783b9fa717a9246e9c676e1cf145c966396c94b8d07b8934afa66df1a53a89eeade16ec58de258bd19cdb777c26501';
DELETE Nodes WHERE UserHash = '1614d0ef0019239571614b9d09bf5700563b5ebecc69a3f8e1b82c06df169bb89382f7e92cad1a3921482d560b67e78157e9b85b57f557310d97a66a526e7807';
DELETE Nodes WHERE UserHash = '5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf';

DELETE UserHashTable WHERE UserHash = '27590a1ba0a96ce95c8681e81181ecca27783b9fa717a9246e9c676e1cf145c966396c94b8d07b8934afa66df1a53a89eeade16ec58de258bd19cdb777c26501';
DELETE UserHashTable WHERE UserHash = '1614d0ef0019239571614b9d09bf5700563b5ebecc69a3f8e1b82c06df169bb89382f7e92cad1a3921482d560b67e78157e9b85b57f557310d97a66a526e7807';
DELETE UserHashTable WHERE UserHash = '5607b3bb2ab8ca6338eb483699414c29697a687ce6134944f8c6f302e0db1faa2c04b44bd1274a191ee633be7d6149ce4d5189d9b372fa8edb0d5597cce680cf';

DELETE Accounts WHERE Username = 'tresearchTagServiceSQLDAOshould1@tresearch.system';
DELETE Accounts WHERE Username = 'tresearchTagServiceSQLDAOshould2@tresearch.system';
DELETE Accounts WHERE Username = 'tresearchTagServiceSQLDAOshould3@tresearch.system';

DROP PROCEDURE IF EXISTS DAOIntegrationTagInitializeProcedure;