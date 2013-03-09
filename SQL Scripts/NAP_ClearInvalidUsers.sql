-- Clear invalid users

USE bodb_NAP

DECLARE @TempTbl	TABLE(UserId INT)

INSERT @TempTbl (UserId)
SELECT u.UserId
FROM
	Users u WITH (NOLOCK)
	INNER JOIN bodb02.dbo.Customer c WITH (NOLOCK)
		ON u.UserId = c.custid

DELETE UserFunction
WHERE
	UserId NOT IN (SELECT UserId
				   FROM
					   @TempTbl)

DELETE UserSite
WHERE
	UserId NOT IN (SELECT UserId
				   FROM
					   @TempTbl)

DELETE Users
WHERE
	UserId NOT IN (SELECT UserId
				   FROM
					   @TempTbl)