create table [UserAssessmentResult] (
    id int identity primary key,
    userId int not null,
    assessmentId int not null,
    passed bit not null,
    taken date not null,
    constraint fk_UserAssessmentResult_User FOREIGN KEY (userId)
        references [User] ([id]),
    constraint fk_UserAssessmentResult_Assessment FOREIGN KEY (assessmentId)
        references [Assessment] ([id])
)
