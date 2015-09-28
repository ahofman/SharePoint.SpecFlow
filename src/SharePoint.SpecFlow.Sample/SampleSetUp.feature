Feature: Some sample feature

Scenario: Some sample set up
	Given there is a DocumentLibrary list called "952_DocLib1" by user as_sitecoll in site "http://rp2013-3:113"
	And the list has a Document Set called "450_DocSetA"
	And the list has a workflow associated
	| WorkflowId                           | WorkflowAssociationName | WorkflowHistoryListName | WorkflowTasksListName | AssociationData                                                                                                                                                                                           | AutoStartChange | AutoStartCreate |
	| 79a21da3-a5ad-4b7e-b7f6-a28b85fa31eb | RP Submit Stub          | Workflow History        | Tasks                 | <SubmitWorkflowAssociationData xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"><WorkflowType>SubmitStub</WorkflowType></SubmitWorkflowAssociationData> | true            | true            |
	And there is a DocumentLibrary list called "450_DocLib2"
	And the list has a Document Set called "450_DocSetB"
	And the list has a workflow associated
	| WorkflowId                           | WorkflowAssociationName | WorkflowHistoryListName | WorkflowTasksListName | AssociationData                                                                                                                                                                                           | AutoStartChange | AutoStartCreate |
	| 79a21da3-a5ad-4b7e-b7f6-a28b85fa31eb | RP Submit Stub          | Workflow History        | Tasks                 | <SubmitWorkflowAssociationData xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"><WorkflowType>SubmitStub</WorkflowType></SubmitWorkflowAssociationData> | true            | true            |
	And there is a file with contents "abc" at server relative url "/450_DocLib1/450_DocSetA/450_blah.txt"
	And there is a file with contents "abc" at server relative url "/450_DocLib2/450_DocSetB/450_blah2.txt"
	And the file is checked out by user as_sitecoll
	When the file is copied to "/450_DocLib1/450_DocSetA/another_copy.txt"
	#And the list called "RecordPointObjectQueue" contains 0 items
