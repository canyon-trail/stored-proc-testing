create table [UserAssessment] (
    id int identity primary key,
    userId int not null,
    assessmentId int not null,
    passed bit not null,
    lastTaken date not null,

    constraint fk_UserAssessment_User FOREIGN KEY (userId)
        references [User] ([id]),
    constraint fk_UserAssessment_Assessment FOREIGN KEY (assessmentId)
        references [Assessment] ([id])
);
