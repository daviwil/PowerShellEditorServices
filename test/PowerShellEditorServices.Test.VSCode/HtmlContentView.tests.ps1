#
# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#

Import-Module $ -Scope Global -Force

function CreateMockPSEditorObject {
    # TODO: Steps
    # 1. Create real $psEditor object
    # 2.
}

Describe "HtmlContentView feature" {

    $expectedViewTitle = "My View"

    It "creates a new view" {
        # $psEditor = @{}
        $view = New-VSCodeHtmlContentView -Title $expectedViewTitle
        $ctx = $psEditor.Mocks.MessageSender.ExpectRequest("powerShell/newCustomView") # TODO: Use type?
        $ctx.Request.Title | Should Be $expectedViewTitle
    }

    It "creates a new view and shows in the specified column" {

    }
}
