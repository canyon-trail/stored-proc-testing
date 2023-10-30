CREATE PROCEDURE AddAssessment
    @userId int,
    @assessmentId int,
    @taken date,
    @passed bit
AS
BEGIN
    declare @assessmentRow table (
        userId int not null,
        assessmentId int not null,
        taken date not null,
        passed bit not null
    );

    insert into @assessmentRow values (@userId, @assessmentId, @taken, @passed);

    merge [UserAssessment] tgt
    using @assessmentRow src
    on tgt.userId = src.userId and tgt.assessmentId = src.assessmentId
    when matched then update set passed = src.passed, lastTaken = src.taken
    when not matched then insert
        (userId, assessmentId, passed, lastTaken)
        values (src.userId, src.assessmentId, src.passed, src.taken)
    ;

    insert into [UserAssessmentResult]
        (userId, assessmentId, passed, taken)
    values (@userId, @assessmentId, @passed, @taken)
END
